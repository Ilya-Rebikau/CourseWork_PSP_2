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
        Task<GaussHelperModel> SubstractRowsAndGetResult([Body] GaussHelperModel data);
    }
}
