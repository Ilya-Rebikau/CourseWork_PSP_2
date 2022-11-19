namespace CourseWork.DistributionAPI.Controllers
{
    using CourseWork.DistributionAPI.Attributes;
    using CourseWork.DistributionAPI.Interfaces;
    using CourseWork.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
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
        /// Лист с данными от вычислительных серверов.
        /// </summary>
        private List<GaussHelperModel> _dataFromServers;

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
            var vectorNumbers = startData.Vector.Numbers;
            for (int i = 0; i < matrixNumbers.Length; i++)
            {
                matrixNumbers[i] = matrixNumbers[i].Append(vectorNumbers[i]).ToArray();
            }

            var serversCount = _servers.Count;
            matrixNumbers = await DirectMotion(matrixNumbers, serversCount);
            for (int i = 0; i < matrixNumbers.Length; i++)
            {
                vectorNumbers[i] = matrixNumbers[i].Last();
                Array.Resize(ref matrixNumbers[i], matrixNumbers.Length);
            }

            ReverseMotion(matrixNumbers, vectorNumbers, serversCount);
            return new DataModel { Vector = new Vector(vectorNumbers) };
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
        private async Task<float[][]> DirectMotion(float[][] matrixNumbers, int serversCount)
        {
            var matrixLength = matrixNumbers.Length;
            var tasksForSubstract = new List<Task>();
            for (int i = 0, k = 0; i < matrixLength; i++, k++)
            {
                if (k == serversCount)
                {
                    k = 0;
                }

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

                tasksForSubstract.Clear();
                _dataFromServers = new List<GaussHelperModel>();
                for (int z = 0; z < serversCount; z++)
                {
                    var data = new GaussHelperModel
                    {
                        Rows = rows[z],
                        SubrstractedRow = matrixNumbers[i],
                        StartIndex = i
                    };

                    tasksForSubstract.Add(SubstractRowsAndGetResult(z, data));
                }

                await Task.WhenAll(tasksForSubstract.ToArray());
                _dataFromServers = _dataFromServers.OrderBy(d => d.Rows.Keys.First()).ToList();
                matrixNumbers = Array.Empty<float[]>();
                for (int z = 0, v = 0, x = 0; x < matrixLength; z++, x++)
                {
                    if (z == serversCount)
                    {
                        v++;
                        z = 0;
                    }

                    if (_dataFromServers[z].Rows.Count != v && !matrixNumbers.Contains(_dataFromServers[z].Rows.Values.ElementAt(v)))
                    {
                        matrixNumbers = matrixNumbers.Append(_dataFromServers[z].Rows.Values.ElementAt(v)).ToArray();
                    }
                }
            }

            return matrixNumbers;
        }

        private async Task SubstractRowsAndGetResult(int z, GaussHelperModel data)
        {
            var res = await _servers[z].SubstractRowsAndGetResult(data);
            _dataFromServers.Add(res);
        }
    }
}
