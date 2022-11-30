namespace CourseWork.Web.Controllers
{
    using CourseWork.Models;
    using CourseWork.Web.Attributes;
    using CourseWork.Web.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер для работы с отправкой и загрузкой файлов.
    /// </summary>
    [ExceptionFilter]
    public class FilesController : Controller
    {
        /// <summary>
        /// Сервис для сериализации и десериализации.
        /// </summary>
        private readonly ISerializer _serializer;

        /// <summary>
        /// HttpClient.
        /// </summary>
        private readonly IDistributionHttpClient _httpClient;

        /// <summary>
        /// Путь к папке с файлами.
        /// </summary>
        private readonly string _pathToFiles;

        /// <summary>
        /// Инициализирует новый объект класса <see cref="FilesController"/>.
        /// </summary>
        /// <param name="httpClient">HttpClient.</param>
        /// <param name="environment">IWebHostEnvironment.</param>
        /// <param name="serializer">Сервис для сериализации и десериализации.</param>
        public FilesController(IDistributionHttpClient httpClient, IWebHostEnvironment environment, ISerializer serializer)
        {
            _httpClient = httpClient;
            _pathToFiles = Path.Combine(environment.WebRootPath, "files");
            _serializer = serializer;
        }

        /// <summary>
        /// Отправляет данные матрицы и вектора на сервер и получает результат решения СЛАУ.
        /// </summary>
        /// <param name="matrixFileName">Имя файла с матрицей.</param>
        /// <param name="vectorFileName">Имя файла с вектором.</param>
        /// <returns>Файл с результатом.</returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> SendMatrixAndVectorToServer(string number)
        {
            var matrix = await _serializer.DeserializeMatrix(Path.Combine(_pathToFiles, $"A{number}" + ".txt"));
            var vector = await _serializer.DeserializeVector(Path.Combine(_pathToFiles, $"B{number}" + ".txt"));
            var data = new DataModel
            {
                Matrix = matrix,
                Vector = vector,
            };

            var result = await _httpClient.SendFileToServer(data);
            return File(_serializer.SerializeVector(result.Vector), "text/plain", $"X{number}.txt");
        }
    }
}
