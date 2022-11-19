namespace CourseWork.Models
{
    /// <summary>
    /// Представляет модель матрицы.
    /// </summary>
    public class Matrix
    {
        /// <summary>
        /// Инициализирует новый объект класса <see cref="Matrix"/>.
        /// </summary>
        /// <param name="numbers">Числа матрицы.</param>
        /// <exception cref="ArgumentException">ArgumentException в случае если матрица не квадратная.</exception>
        public Matrix(float[][] numbers)
        {
            if (numbers.Length != numbers[0].Length)
            {
                throw new ArgumentException("Матрица не квадратная!");
            }

            Numbers = numbers;
            Size = numbers.Length;
        }

        /// <summary>
        /// Получает или задаёт размер матрицы.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Получает или задаёт числа матрицы.
        /// </summary>
        public float[][] Numbers { get; set; }
    }
}