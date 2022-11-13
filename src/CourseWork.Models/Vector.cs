using System.Text;

namespace CourseWork.Models
{
    /// <summary>
    /// Представляет модель вектора
    /// </summary>
    public class Vector
    {
        /// <summary>
        /// Инициализирует новый объект класса <see cref="Vector"/>.
        /// </summary>
        /// <param name="numbers">Числа вектора.</param>
        public Vector(float[] numbers)
        {
            Numbers = numbers;
            Size = numbers.Length;
        }

        /// <summary>
        /// Получает или задаёт числа вектора.
        /// </summary>
        public float[] Numbers { get; set; }

        /// <summary>
        /// Получает или задаёт размер вектора.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Получает или задаёт точность для сравнения векторов.
        /// </summary>
        public float Precision { get; set; } = 0.005F;

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType())
            {
                return false;
            }

            var vector = (Vector)obj;
            if (vector.Size != Size)
            {
                return false;
            }

            for (int i = 0; i < Size; i++)
            {
                var firstValue = vector.Numbers[i];
                var secondValue = Numbers[i];
                if (Math.Abs(firstValue - secondValue) > Precision)
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Size.GetHashCode();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < Numbers.Length; i++)
            {
                sb.AppendLine($"{Numbers[i]}");
            }

            return sb.ToString();
        }
    }
}
