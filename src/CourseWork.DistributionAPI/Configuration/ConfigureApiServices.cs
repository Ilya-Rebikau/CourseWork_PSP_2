namespace CourseWork.DistributionAPI.Configuration
{
    using CourseWork.DistributionAPI.Interfaces;
    using CourseWork.DistributionAPI.Services;
    using Microsoft.Extensions.DependencyInjection;

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
            services.AddSingleton<IFactory<IComputingHttpClient>, HttpClientsFactory>();
            return services;
        }
    }
}
