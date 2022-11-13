namespace CourseWork.ComputingAPI.Controllers
{
    using CourseWork.ComputingAPI.Attributes;
    using CourseWork.ComputingAPI.Math;
    using CourseWork.Models;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Выполняет математические вычисления.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [ExceptionFilter]
    [DisableRequestSizeLimit]
    public class MathsController : ControllerBase
    {
        /// <summary>
        /// Вычисляет конечный Х для строки.
        /// </summary>
        /// <param name="data">Модель с промежуточным Х и его множителем.</param>
        /// <returns>Конечный Х.</returns>
        [HttpPost("CalculateX")]
        public float CalculateX([FromBody] GaussHelperModel data)
        {
            return GaussMethodHelper.CalculateX(data.IntermediateX, data.MatrixNumber);
        }

        /// <summary>
        /// Вычисляет промежуточный Х.
        /// </summary>
        /// <param name="data">Модель с промежуточным предыдущим Х, последним вычисленным Х и множителем перед промежуточным Х.</param>
        /// <returns>Промежуточный Х.</returns>
        [HttpPost("CalculateIntermediateX")]
        public float CalculateIntermediateX([FromBody] GaussHelperModel data)
        {
            return GaussMethodHelper.CalculateIntermediateX(data.IntermediateX, data.CurrentX, data.MatrixNumber);
        }

        /// <summary>
        /// Вычитает строку из заданных строк.
        /// </summary>
        /// <param name="data">Модель со строками, стартовым индексом и вычитаемой строкой.</param>
        /// <returns>Модель с полученными строками.</returns>
        [HttpPost("SubstractRowsAndGetResult")]
        public GaussHelperModel SubstractRowsAndGetResult([FromBody] GaussHelperModel data)
        {
            var helper = new GaussMethodHelper();
            helper.Rows = data.Rows;
            helper.SubstractedRow = data.SubrstractedRow;
            helper.SubstractRows(data.StartIndex);
            return new GaussHelperModel { Rows = helper.Rows };
        }
    }
}
