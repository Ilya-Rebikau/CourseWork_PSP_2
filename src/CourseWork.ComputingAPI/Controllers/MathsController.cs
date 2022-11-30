namespace CourseWork.ComputingAPI.Controllers
{
    using CourseWork.ComputingAPI.Attributes;
    using CourseWork.ComputingAPI.Interfaces;
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
        /// Помощник метода Гаусса.
        /// </summary>
        private readonly IGaussMethodHelper _helper;

        /// <summary>
        /// Инициализирует новый объект класса <see cref="MathsController"/>.
        /// </summary>
        /// <param name="helper">Помощник метода Гаусса.</param>
        public MathsController(IGaussMethodHelper helper)
        {
            _helper = helper;
        }

        /// <summary>
        /// Вычитает строку из заданных строк.
        /// </summary>
        /// <param name="data">Модель со строками, стартовым индексом и вычитаемой строкой.</param>
        /// <returns>Модель с полученными строками.</returns>
        [HttpPost("SubstractRowsAndGetResult")]
        public void SubstractRowsAndGetResult([FromBody] GaussHelperModel data)
        {
            _helper.SubstractedRow = data.SubstractedRow;
            _helper.SubstractRows(data.StartIndex);
        }

        /// <summary>
        /// Отсылает строки в помощник.
        /// </summary>
        /// <param name="data">Строки.</param>
        [HttpPost("SendRows")]
        public void SendRows([FromBody] GaussHelperModel data)
        {
            _helper.Rows = data.Rows;
        }

        /// <summary>
        /// Получает разрешающую строку и её индекс в матрице.
        /// </summary>
        /// <param name="index">Индекс разрешающей строки в своём блоке.</param>
        /// <returns>Разрешающая строка и её индекс в матрице.</returns>
        [HttpPost("GetSubstractedRowAndIndex")]
        public GaussHelperModel GetSubstractedRowAndIndex(int index)
        {
            return new GaussHelperModel
            {
                SubstractedRow = _helper.Rows.Values.ElementAt(index),
                StartIndex = _helper.Rows.Keys.ElementAt(index)
            };
        }

        /// <summary>
        /// Получает результат преобразований строк.
        /// </summary>
        /// <returns>Преобразованные строки.</returns>
        [HttpGet("GetResult")]
        public GaussHelperModel GetResult()
        {
            return new GaussHelperModel { Rows = _helper.Rows };
        }
    }
}
