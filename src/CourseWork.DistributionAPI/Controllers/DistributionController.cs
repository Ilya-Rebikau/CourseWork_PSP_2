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
        /// Массив HttpClients для всех вычислительных серверов.
        /// </summary>
        private readonly IComputingHttpClient[] _servers;

        List<GaussHelperModel> _dataFromServers;

        float[] _vectorX;

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
            _servers = new IComputingHttpClient[]
            {
                firstHttpClient, secondHttpClient, thirdHttpClient
            };
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

            var serversCount = _servers.Length;
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

            for (int i = 0; i < matrixLength; i++)
            {
                vectorNumbers[i] = matrixNumbers[i].Last();
                Array.Resize(ref matrixNumbers[i], matrixLength);
            }

            _vectorX = vectorNumbers;
            var tasksForCalculateIntermediateX = new List<Task>();
            for (int i = vectorNumbers.Length - 1, k = serversCount - 1; i >= 0; i--)
            {
                if (k == -1)
                {
                    k = serversCount - 1;
                }

                _vectorX[i] = await _servers[k].CalculateX(new GaussHelperModel
                {
                    IntermediateX = _vectorX[i],
                    MatrixNumber = matrixNumbers[i][i]
                });

                k--;
                tasksForCalculateIntermediateX.Clear();
                for (int j = i - 1; j >= 0; j--, k--)
                {
                    if (k == -1)
                    {
                        k = serversCount - 1;
                    }

                    var data = new GaussHelperModel
                    {
                        CurrentX = _vectorX[i],
                        IntermediateX = _vectorX[j],
                        MatrixNumber = matrixNumbers[j][i]
                    };
                    tasksForCalculateIntermediateX.Add(CalculateIntermediateX(k, data, j));
                }

                await Task.WhenAll(tasksForCalculateIntermediateX.ToArray());
            }

            return new DataModel { Vector = new Vector(_vectorX)};
        }

        private async Task SubstractRowsAndGetResult(int z, GaussHelperModel data)
        {
            var res = await _servers[z].SubstractRowsAndGetResult(data);
            _dataFromServers.Add(res);
        }

        private async Task CalculateIntermediateX(int k, GaussHelperModel data, int j)
        {
            var res = await _servers[k].CalculateIntermediateX(data);
            _vectorX[j] = res;
        }
    }
}
