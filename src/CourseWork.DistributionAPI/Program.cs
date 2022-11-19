using CourseWork.DistributionAPI.Configuration;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CourseWork.IntegrationTests")]

namespace CourseWork.DistributionAPI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddAPIServices(builder.Configuration);

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}