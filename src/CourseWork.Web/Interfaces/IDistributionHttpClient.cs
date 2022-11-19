namespace CourseWork.Web.Interfaces
{
    using CourseWork.Models;
    using RestEase;

    /// <summary>
    /// HttpClient для распределительного сервера.
    /// </summary>
    public interface IDistributionHttpClient
    {
        /// <summary>
        /// Отправляет данные файлов на сервер.
        /// </summary>
        /// <param name="fileData">Данные файлов.</param>
        /// <returns>Результат с вектором ответов.</returns>
        [Post("Distribution/DistributeSlae")]
        Task<DataModel> SendFileToServer([Body] DataModel fileData);
    }
}
