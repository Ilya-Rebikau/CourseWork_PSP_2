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
        /// Вычисляет конечный Х для строки.
        /// </summary>
        /// <param name="data">Модель с промежуточным Х и его множителем.</param>
        /// <returns>Конечный Х.</returns>
        [Post("Maths/CalculateX")]
        Task<float> CalculateX([Body] GaussHelperModel data);


        /// <summary>
        /// Вычисляет промежуточный Х.
        /// </summary>
        /// <param name="data">Модель с промежуточным предыдущим Х, последним вычисленным Х и множителем перед промежуточным Х.</param>
        /// <returns>Промежуточный Х.</returns>
        [Post("Maths/CalculateIntermediateX")]
        Task<float> CalculateIntermediateX([Body] GaussHelperModel data);

        /// <summary>
        /// Вычитает строку из заданных строк.
        /// </summary>
        /// <param name="data">Модель со строками, стартовым индексом и вычитаемой строкой.</param>
        /// <returns>Модель с полученными строками.</returns>
        [Post("Maths/SubstractRowsAndGetResult")]
        Task<GaussHelperModel> SubstractRowsAndGetResult([Body] GaussHelperModel data);
    }
}
