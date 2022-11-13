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
        /// Массив HttpClients для всех вычислительных серверов.
        /// </summary>
        private readonly IComputingHttpClient[] servers;

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributionController"/> class.
        /// </summary>
        /// <param name="firstHttpClient">HttpClient для первого вычислительного сервера.</param>
        /// <param name="secondHttpClient">HttpClient для второго вычислительного сервера.</param>
        /// <param name="thirdHttpClient">HttpClient для третьего вычислительного сервера.</param>
        public DistributionController(
            IFirstComputingHttpClient firstHttpClient,
            ISecondComputingHttpClient secondHttpClient,
            IThirdComputingHttpClient thirdHttpClient)
        {
            servers = new IComputingHttpClient[]
            {
                firstHttpClient, secondHttpClient, thirdHttpClient
            };
        }

        /// <summary>
        /// Распределяет данные между вычислительными серверами и возвращает результат.
        /// </summary>
        /// <param name="data">Данные с матрицей и вектором.</param>
        /// <returns>Результат с вектором Х.</returns>
        [HttpPost("DistributeSlae")]
        public async Task<DataModel> DistributeSlaeAndGetResult([FromBody] DataModel startData)
        {
            var matrixNumbers = startData.Matrix.Numbers;
            var vectorNumbers = startData.Vector.Numbers;


            matrixNumbers = new float[][]
            {
                new float[] { 4, 7, 13 },
                new float[] { 9, 15, 12 },
                new float[] { 5, 1, 8 },
            };
            vectorNumbers = new float[] { 14, 26, 32 };


            for (int i = 0; i < matrixNumbers.Length; i++)
            {
                matrixNumbers[i] = matrixNumbers[i].Append(vectorNumbers[i]).ToArray();
            }

            int serversCount = servers.Length;
            for (int i = 0, k = 0; i < matrixNumbers.Length; i++, k++)
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

                for (int j = 0; j < matrixNumbers.Length;)
                {
                    for (int z = 0; z < serversCount; z++)
                    {
                        if (j == matrixNumbers.Length)
                        {
                            break;
                        }

                        rows[z].Add(j, matrixNumbers[j]);
                        j++;
                    }
                }

                var dataFromServers = new List<GaussHelperModel>();
                for (int z = 0; z < serversCount; z++)
                {
                    var data = new GaussHelperModel
                    {
                        Rows = rows[z],
                        SubrstractedRow = matrixNumbers[i],
                        StartIndex = i
                    };

                    dataFromServers.Add(await servers[z].SubstractRowsAndGetResult(data));
                }

                var totalNumber = matrixNumbers.Length;
                matrixNumbers = Array.Empty<float[]>();
                for (int z = 0, v = 0, x = 0; x < totalNumber; z++, x++)
                {
                    if (z == serversCount)
                    {
                        v++;
                        z = 0;
                    }

                    if (dataFromServers[z].Rows.Count != v && !Contains(matrixNumbers, dataFromServers[z].Rows.Values.ElementAt(v)))
                    {
                        matrixNumbers = matrixNumbers.Append(dataFromServers[z].Rows.Values.ElementAt(v)).ToArray();
                    }
                }
            }

            for (int i = 0; i < matrixNumbers.Length; i++)
            {
                vectorNumbers[i] = matrixNumbers[i].Last();
                Array.Resize(ref matrixNumbers[i], matrixNumbers.Length);
            }

            var vectorX = vectorNumbers;
            for (int i = vectorNumbers.Length - 1, k = serversCount - 1; i >= 0; i--, k--)
            {
                if (k == 0)
                {
                    k = serversCount - 1;
                }

                vectorX[i] = await servers[k].CalculateX(new GaussHelperModel
                {
                    IntermediateX = vectorX[i],
                    MatrixNumber = matrixNumbers[i][i]
                });
                var currentX = vectorX[i];
                var currentIndex = i;
                for (int j = i - 1; j >= 0; j--)
                {
                    vectorX[j] = await servers[k].CalculateIntermediateX(new GaussHelperModel
                    {
                        CurrentX = currentX,
                        IntermediateX = vectorX[j],
                        MatrixNumber = matrixNumbers[j][currentIndex]
                    });
                }
            }

            return new DataModel { Vector = new Vector(vectorX)};
        }

        /// <summary>
        /// Проверяет содержит ли матрица строку.
        /// </summary>
        /// <param name="matrix">Матрица</param>
        /// <param name="row">Строка.</param>
        /// <returns>True если содержит, false - если нет.</returns>
        private static bool Contains(float[][] matrix, float[] row)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                var x = 0;
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (matrix[i][j] == row[j])
                    {
                        x++;
                    }
                }

                if (x == matrix[i].Length)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
