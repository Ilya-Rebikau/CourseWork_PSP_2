namespace CourseWork.DistributionAPI.Interfaces
{
    using CourseWork.Models;
    using RestEase;

    /// <summary>
    /// Http клиент для вычислительного сервера.
    /// </summary>
    public interface IComputingHttpClient
    {
        /// <summary>
        /// Вычитает строку из заданных строк.
        /// </summary>
        /// <param name="data">Модель со строками, стартовым индексом и вычитаемой строкой.</param>
        /// <returns>Модель с полученными строками.</returns>
        [Post("Maths/SubstractRowsAndGetResult")]
        Task SubstractRowsAndGetResult([Body] GaussHelperModel data);

        /// <summary>
        /// Отсылает строки на вычислительный сервер.
        /// </summary>
        /// <param name="data">Строки.</param>
        [Post("Maths/SendRows")]
        Task SendRows([Body] GaussHelperModel data);

        /// <summary>
        /// Получает разрешающую строку и её индекс в матрице.
        /// </summary>
        /// <param name="index">Индекс разрешающей строки в своём блоке.</param>
        /// <returns>Разрешающая строка и её индекс в матрице.</returns>
        [Post("Maths/GetSubstractedRowAndIndex")]
        Task<GaussHelperModel> GetSubstractedRowAndIndex(int index);

        /// <summary>
        /// Получает результат преобразований строк.
        /// </summary>
        /// <returns>Преобразованные строки.</returns>
        [Get("Maths/GetResult")]
        Task<GaussHelperModel> GetResult();
    }
}
