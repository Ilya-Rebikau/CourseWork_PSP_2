namespace CourseWork.DistributionAPI.Services
{
    using CourseWork.DistributionAPI.Interfaces;
    using RestEase;

    /// <summary>
    /// Фабричный метод для http клиентов.
    /// </summary>
    internal class HttpClientsFactory : IFactory<IComputingHttpClient>
    {
        /// <summary>
        /// Объект IConfiguration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Инициализирует новый объект класса <see cref="HttpClientsFactory"/>.
        /// </summary>
        /// <param name="configuration">Объект IConfiguration.</param>
        public HttpClientsFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Создаёт лист http клиентов.
        /// </summary>
        /// <returns>Лист http клиентов.</returns>
        public List<IComputingHttpClient> CreateList()
        {
            var servers = new List<IComputingHttpClient>();
            var serversCount = int.Parse(_configuration["ServersCount"]);
            for (int i = 0; i < serversCount; i++)
            {
                var baseUrl = _configuration["BaseUrl"] + ":" + (5002 + i);
                servers.Add(RestClient.For<IComputingHttpClient>(baseUrl));
            }

            return servers;
        }
    }
}
