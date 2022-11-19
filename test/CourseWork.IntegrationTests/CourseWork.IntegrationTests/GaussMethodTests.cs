using CourseWork.Models;
using CourseWork.Web.Interfaces;
using CourseWork.Web.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace CourseWork.IntegrationTests
{
    public class GaussMethodTests
    {
        private readonly HttpClient _distributionClient;
        private static readonly string _pathToFiles = @"../../../../../../src/CourseWork.Web/wwwroot/files";
        public GaussMethodTests()
        {
            var distributionServer = new WebApplicationFactory<DistributionAPI.Program>();
            _distributionClient = distributionServer.CreateClient();
            _distributionClient.Timeout = TimeSpan.FromHours(24);
        }

        private static async Task<Matrix> GetTestMatrix(string matrixName)
        {
            ISerializer serializer = new Serializer();
            return await serializer.DeserializeMatrix(Path.Combine(_pathToFiles, matrixName));
        }

        private static async Task<Vector> GetTestVector(string vectorName)
        {
            ISerializer serializer = new Serializer();
            return await serializer.DeserializeVector(Path.Combine(_pathToFiles, vectorName));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public async Task CompareResults_WithFile(string numberOfSlae)
        {
            // Arrange
            var matrix = await GetTestMatrix($"A{numberOfSlae}.txt");
            var vector = await GetTestVector($"B{numberOfSlae}.txt");
            var dataModel = new DataModel
            {
                Matrix = matrix,
                Vector = vector
            };
            var expectedResult = await GetTestVector($"X{numberOfSlae}.txt");

            // Act
            var response = await _distributionClient.PostAsJsonAsync("Distribution/DistributeSlae", dataModel);
            var actualResult = JsonConvert.DeserializeObject<DataModel>(await response.Content.ReadAsStringAsync()).Vector;
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}