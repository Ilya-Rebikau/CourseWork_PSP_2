namespace CourseWork.DistributionAPI.Controllers
{
    using CourseWork.DistributionAPI.Attributes;
    using CourseWork.DistributionAPI.Interfaces;
    using CourseWork.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    /// <summary>
    /// Контроллер для распределения задач.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [ExceptionFilter]
    public class DistributionController : ControllerBase
    {
        /// <summary>
        /// Объект для синхронизации потоков.
        /// </summary>
        private readonly object _objectForLock = new();

        /// <summary>
        /// Лист с данными от вычислительных серверов.
        /// </summary>
        private readonly List<GaussHelperModel> _dataFromServers = new();

        /// <summary>
        /// Http клиенты для вычислительных серверов.
        /// </summary>
        private readonly List<IComputingHttpClient> _servers;

        /// <summary>
        /// Инициализирует новый объект класса <see cref="DistributionController"/>.
        /// </summary>
        /// <param name="factory">Factory to build list of http clients for computing servers.</param>
        public DistributionController(IFactory<IComputingHttpClient> factory)
        {
            _servers = factory.CreateList();
        }

        /// <summary>
        /// Распределяет данные между вычислительными серверами и возвращает результат.
        /// </summary>
        /// <param name="startData">Данные с матрицей и вектором.</param>
        /// <returns>Результат с вектором Х.</returns>
        [HttpPost("DistributeSlae")]
        public async Task<DataModel> DistributeSlaeAndGetResult([FromBody] DataModel startData)
        {
            var matrixNumbers = startData.Matrix.Numbers;
            var matrixLength = matrixNumbers.Length;
            var vectorNumbers = startData.Vector.Numbers;
            var serversCount = _servers.Count;
            JoinMatrixToVector(matrixNumbers, vectorNumbers);
            await SendRowsToServers(matrixNumbers, serversCount, matrixLength);
            await DirectMotion(matrixNumbers, serversCount);
            matrixNumbers = await GetResultsFromServers(matrixNumbers, serversCount, matrixLength);
            SeparateMatrixAndVector(matrixNumbers, vectorNumbers);
            ReverseMotion(matrixNumbers, vectorNumbers, serversCount);
            return new DataModel { Vector = new Vector(vectorNumbers) };
        }

        /// <summary>
        /// Объединяет матрицу и вектор в расширенную матрицу.
        /// </summary>
        /// <param name="matrixNumbers">Матрица.</param>
        /// <param name="vectorNumbers">Вектор.</param>
        private static void JoinMatrixToVector(float[][] matrixNumbers, float[] vectorNumbers)
        {
            for (int i = 0; i < matrixNumbers.Length; i++)
            {
                matrixNumbers[i] = matrixNumbers[i].Append(vectorNumbers[i]).ToArray();
            }
        }

        /// <summary>
        /// Разделяет расширенную матрицу на обычную матрицу и вектор.
        /// </summary>
        /// <param name="matrixNumbers">Расширенная матрица.</param>
        /// <param name="vectorNumbers">Вектор.</param>
        private static void SeparateMatrixAndVector(float[][] matrixNumbers, float[] vectorNumbers)
        {
            for (int i = 0; i < matrixNumbers.Length; i++)
            {
                vectorNumbers[i] = matrixNumbers[i].Last();
                Array.Resize(ref matrixNumbers[i], matrixNumbers.Length);
            }
        }

        /// <summary>
        /// Метод для обратного хода Гаусса.
        /// </summary>
        /// <param name="matrixNumbers">Матрица.</param>
        /// <param name="vectorNumbers">Вектор промежуточных значений Х.</param>
        /// <param name="serversCount">Количество серверов.</param>
        private static void ReverseMotion(float[][] matrixNumbers, float[] vectorNumbers, int serversCount)
        {
            for (int i = vectorNumbers.Length - 1, k = serversCount - 1; i >= 0; i--)
            {
                if (matrixNumbers[i][i] == 0)
                {
                    vectorNumbers[i] = 0;
                }
                else
                {
                    vectorNumbers[i] = vectorNumbers[i] / matrixNumbers[i][i];
                }

                for (int j = i - 1; j >= 0; j--, k--)
                {
                    vectorNumbers[j] = vectorNumbers[j] - matrixNumbers[j][i] * vectorNumbers[i];
                }
            }
        }

        /// <summary>
        /// Метод для распределённого прямого хода Гаусса
        /// </summary>
        /// <param name="matrixNumbers">Числа матрицы.</param>
        /// <param name="serversCount">Количество серверов.</param>
        /// <returns>Новая матрица.</returns>
        private async Task DirectMotion(float[][] matrixNumbers, int serversCount)
        {
            var matrixLength = matrixNumbers.Length;
            var tasksForSubstract = new List<Task>();
            for (int i = 0, k = 0, n = 0; i < matrixLength; i++, k++)
            {
                if (k == serversCount)
                {
                    k = 0;
                    n++;
                }

                var substractedRowAndIndex = await _servers[k].GetSubstractedRowAndIndex(n);
                tasksForSubstract.Clear();
                for (int z = 0; z < serversCount; z++)
                {
                    var data = new GaussHelperModel
                    {
                        SubstractedRow = substractedRowAndIndex.SubstractedRow,
                        StartIndex = substractedRowAndIndex.StartIndex
                    };

                    tasksForSubstract.Add(SubstractRowsAndGetResult(z, data));
                }

                await Task.WhenAll(tasksForSubstract.ToArray());
            }
        }

        /// <summary>
        /// Получить результаты со всех серверов.
        /// </summary>
        /// <param name="matrixNumbers">Матрица.</param>
        /// <param name="serversCount">Количество серверов.</param>
        /// <param name="matrixLength">Длина матрицы.</param>
        /// <returns>Итоговая матрица.</returns>
        private async Task<float[][]> GetResultsFromServers(float[][] matrixNumbers, int serversCount, int matrixLength)
        {
            var tasksForGetResults = new List<Task>();
            for (int i = 0; i < serversCount; i++)
            {
                tasksForGetResults.Add(GetResult(i));
            }

            await Task.WhenAll(tasksForGetResults);
            for (int i = 0, k = 0; i < matrixLength; i++, k++)
            {
                var fullDataFromServers = new Dictionary<int, float[]>();
                for (int j = 0; j < _dataFromServers.Count; j++)
                {
                    for (int v = 0; v < _dataFromServers[j].Rows.Count; v++)
                    {
                        fullDataFromServers.Add(_dataFromServers[j].Rows.Keys.ElementAt(v), _dataFromServers[j].Rows.Values.ElementAt(v));
                    }
                }

                fullDataFromServers = fullDataFromServers.OrderBy(d => d.Key).ToDictionary(d => d.Key, d => d.Value);
                matrixNumbers = fullDataFromServers.Values.ToArray();
            }

            return matrixNumbers;
        }

        /// <summary>
        /// Получает результат в виде обновлённых строк с сервера.
        /// </summary>
        /// <param name="i">Номер сервера.</param>
        /// <returns>Task.</returns>
        private async Task GetResult(int i)
        {
            var result = await _servers[i].GetResult();
            lock (_objectForLock)
            {
                _dataFromServers.Add(result);
            }
        }

        /// <summary>
        /// Отправить блоки строк на сервера.
        /// </summary>
        /// <param name="matrixNumbers">Матрица.</param>
        /// <param name="serversCount">Количество серверов</param>
        /// <param name="matrixLength">Размерность матрицы.</param>
        /// <returns>Task.</returns>
        private async Task SendRowsToServers(float[][] matrixNumbers, int serversCount, int matrixLength)
        {
            var tasksForSendRows = new List<Task>();
            var rows = new List<Dictionary<int, float[]>>();
            for (int z = 0; z < serversCount; z++)
            {
                rows.Add(new Dictionary<int, float[]>());
            }

            for (int j = 0; j < matrixLength;)
            {
                for (int z = 0; z < serversCount; z++)
                {
                    if (j == matrixLength)
                    {
                        break;
                    }

                    rows[z].Add(j, matrixNumbers[j]);
                    j++;
                }
            }

            for (int z = 0; z < serversCount; z++)
            {
                var data = new GaussHelperModel
                {
                    Rows = rows[z],
                };

                tasksForSendRows.Add(SendRows(z, data));
            }

            await Task.WhenAll(tasksForSendRows);
        }

        /// <summary>
        /// На сервере вычесть разрешающую строку из его строк.
        /// </summary>
        /// <param name="z">Номер сервера.</param>
        /// <param name="data">Разрешающая строка и её индекс в матрице.</param>
        /// <returns>Task.</returns>
        private async Task SubstractRowsAndGetResult(int z, GaussHelperModel data)
        {
            await _servers[z].SubstractRowsAndGetResult(data);
        }

        /// <summary>
        /// Отправить строки на сервер.
        /// </summary>
        /// <param name="z">Номер сервера.</param>
        /// <param name="data">Строки.</param>
        /// <returns>Task.</returns>
        private async Task SendRows(int z, GaussHelperModel data)
        {
            await _servers[z].SendRows(data);
        }
    }
}
