namespace CourseWork.Web.Configuration
{
    using CourseWork.Models;
    using CourseWork.Web.Interfaces;
    using CourseWork.Web.Services;
    using Microsoft.Extensions.DependencyInjection;
    using RestEase;

    /// <summary>
    /// Конфигурация веб сервисов.
    /// </summary>
    public static class ConfigureWebServices
    {
        /// <summary>
        /// Метод расширения для IServiceCollection для добавления веб сервисов.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="configuration">Конфигурация.</param>
        /// <returns>Добавленные сервисы.</returns>
        public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();
            services.AddScoped<ISerializer, Serializer>();
            services.AddHttpClient();
            services.AddScoped(scope =>
            {
                var baseUrl = configuration["DistributionApiAddress"];
                return RestClient.For<IDistributionHttpClient>(baseUrl);
            });

            return services;
        }
    }
}
