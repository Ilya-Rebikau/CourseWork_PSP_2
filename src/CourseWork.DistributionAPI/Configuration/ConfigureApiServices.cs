namespace CourseWork.DistributionAPI.Configuration
{
    using CourseWork.DistributionAPI.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using RestEase;

    /// <summary>
    /// Конфигурация сервисов API.
    /// </summary>
    public static class ConfigureApiServices
    {
        /// <summary>
        /// Метод расширения для IServiceCollection для добавления API сервисов.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="configuration">Конфигурация.</param>
        /// <returns>Добавленные сервисы.</returns>
        public static IServiceCollection AddAPIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddControllers();
            services.AddHttpClient();
            services.AddScoped(scope =>
            {
                var baseUrl = configuration["FirstComputingApiAddress"];
                return RestClient.For<IFirstComputingHttpClient>(baseUrl);
            });
            services.AddScoped(scope =>
            {
                var baseUrl = configuration["SecondComputingApiAddress"];
                return RestClient.For<ISecondComputingHttpClient>(baseUrl);
            });
            services.AddScoped(scope =>
            {
                var baseUrl = configuration["ThirdComputingApiAddress"];
                return RestClient.For<IThirdComputingHttpClient>(baseUrl);
            });
            services.AddScoped(scope =>
            {
                var baseUrl = configuration["FourthComputingApiAddress"];
                return RestClient.For<IFourthComputingHttpClient>(baseUrl);
            });
            services.AddScoped(scope =>
            {
                var baseUrl = configuration["FifthComputingApiAddress"];
                return RestClient.For<IFifthComputingHttpClient>(baseUrl);
            });
            services.AddScoped(scope =>
            {
                var baseUrl = configuration["SixthComputingApiAddress"];
                return RestClient.For<ISixthComputingHttpClient>(baseUrl);
            });
            return services;
        }
    }
}
