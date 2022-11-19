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
        /// Вычитает строку из заданных строк.
        /// </summary>
        /// <param name="data">Модель со строками, стартовым индексом и вычитаемой строкой.</param>
        /// <returns>Модель с полученными строками.</returns>
        [HttpPost("SubstractRowsAndGetResult")]
        public GaussHelperModel SubstractRowsAndGetResult([FromBody] GaussHelperModel data)
        {
            var helper = new GaussMethodHelper
            {
                Rows = data.Rows,
                SubstractedRow = data.SubrstractedRow
            };
            helper.SubstractRows(data.StartIndex);
            data.SubrstractedRow = null;
            data.Rows = helper.Rows;
            return data;
        }
    }
}
