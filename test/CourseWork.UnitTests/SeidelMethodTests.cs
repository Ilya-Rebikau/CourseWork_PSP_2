using CourseWork.Models;
using CourseWork.Web.Interfaces;
using CourseWork.Web.Services;

namespace CourseWork.UnitTests
{
    public class Tests
    {
        private static readonly string _pathToFiles = @"../../../../../src/CourseWork.Web/wwwroot/files";

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

        [Test]
        public async Task CompareResult_WithSeidelMethod_First()
        {
            // Arrange
            var matrix = await GetTestMatrix("A1");
            var vector = await GetTestVector("B1");
            var seidelMethod = new SeidelMethod();

            // Act
            var seidelVector = seidelMethod.Solve(matrix, vector);
            var vectorFromFile = await GetTestVector("X1");

            // Assert
            Assert.That(seidelVector, Is.EqualTo(vectorFromFile));
        }

        [Test]
        public async Task CompareResult_WithSeidelMethod_Second()
        {
            // Arrange
            var matrix = await GetTestMatrix("A2");
            var vector = await GetTestVector("B2");
            var seidelMethod = new SeidelMethod();

            // Act
            var seidelVector = seidelMethod.Solve(matrix, vector);
            var vectorFromFile = await GetTestVector("X2");

            // Assert
            Assert.That(seidelVector, Is.EqualTo(vectorFromFile));
        }

        [Test]
        public void LoadTest()
        {
            // Arrange and Act
            static void testAction()
            {
                var matrix = new Matrix(new float[int.MaxValue][]);
                for (int i = 0; i < int.MaxValue; i++)
                {
                    matrix.Numbers[i] = new float[int.MaxValue];
                }
            }

            // Assert
            Assert.Throws<OutOfMemoryException>(testAction);
        }
    }
}