namespace CourseWork.Web.Services
{
    using System.Runtime.Serialization.Formatters.Binary;
    using CourseWork.Models;
    using CourseWork.Web.Interfaces;

    /// <summary>
    /// Сервис для сериализация и десериализации.
    /// </summary>
    internal class Serializer : ISerializer
    {
        /// <inheritdoc/>
        public byte[] SerializeVector(Vector vector)
        {
            var bf = new BinaryFormatter();
            using var ms = new MemoryStream();
            bf.Serialize(ms, vector.ToString());
            return ms.ToArray();
        }

        /// <inheritdoc/>
        public async Task<Matrix> DeserializeMatrix(string path)
        {
            var initNumbers = true;
            float[][] numbers = Array.Empty<float[]>();
            using var reader = new StreamReader(path);
            string line;
            int i = 0;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                string[] elements = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (initNumbers)
                {
                    numbers = new float[elements.Length][];
                    initNumbers = false;
                }

                numbers[i] = new float[elements.Length];
                for (int j = 0; j < elements.Length; j++)
                {
                    numbers[i][j] = float.Parse(elements[j]);
                }

                i++;
            }

            return new Matrix(numbers);
        }

        /// <inheritdoc/>
        public async Task<Vector> DeserializeVector(string path)
        {
            var numbers = new List<float>();
            using var reader = new StreamReader(path);
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                numbers.Add(float.Parse(line));
            }

            return new Vector(numbers.ToArray());
        }
    }
}
