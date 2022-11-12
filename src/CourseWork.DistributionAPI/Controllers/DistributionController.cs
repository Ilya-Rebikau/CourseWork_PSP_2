namespace CourseWork.DistributionAPI.Controllers
{
    using CourseWork.DistributionAPI.Attributes;
    using CourseWork.Models;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер для распределения задач.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [ExceptionFilter]
    public class DistributionController : ControllerBase
    {
        /// <summary>
        /// Распределяет данные между серверами.
        /// </summary>
        /// <param name="data">Данные с матрицей и вектором.</param>
        /// <returns>Результат.</returns>
        [HttpPost("DistributeFiles")]
        public async Task<DataModel> DistributeFiles([FromBody] DataModel data)
        {
            return data;
        }
    }
}
