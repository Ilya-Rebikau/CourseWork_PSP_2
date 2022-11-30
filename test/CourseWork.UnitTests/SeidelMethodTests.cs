using CourseWork.Models;
using CourseWork.Web.Interfaces;
using CourseWork.Web.Services;

namespace CourseWork.UnitTests
{
    public class SeidelMethodTests
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
            var matrix = await GetTestMatrix("A1.txt");
            var vector = await GetTestVector("B1.txt");
            var seidelMethod = new SeidelMethod();

            // Act
            void test() => seidelMethod.Solve(matrix, vector);

            // Assert
            Assert.Throws<InvalidOperationException>(test);
        }

        [Test]
        public async Task CompareResult_WithSeidelMethod_Second()
        {
            // Arrange
            var matrix = await GetTestMatrix("A2.txt");
            var vector = await GetTestVector("B2.txt");
            var seidelMethod = new SeidelMethod();

            // Act
            void test() => seidelMethod.Solve(matrix, vector);

            // Assert
            Assert.Throws<InvalidOperationException>(test);
        }
        [Test]
        public async Task CompareResult_WithSeidelMethod_Third()
        {
            // Arrange
            var matrix = await GetTestMatrix("A3.txt");
            var vector = await GetTestVector("B3.txt");
            var seidelMethod = new SeidelMethod();

            // Act
            void test() => seidelMethod.Solve(matrix, vector);

            // Assert
            Assert.Throws<InvalidOperationException>(test);
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