namespace CourseWork.ComputingAPI.Math
{
    /// <summary>
    /// Содержит необходимые методы для распределённого решения СЛАУ методом Гаусса с циклическим размещением по строкам.
    /// </summary>
    internal class GaussMethodHelper
    {
        /// <summary>
        /// Строки с их индексами, из которых необходимо вычесть вычитаемую строку.
        /// </summary>
        public Dictionary<int, float[]> Rows { get; set; }

        /// <summary>
        /// Вычитаемая строка из остальных строк.
        /// </summary>
        public float[] SubstractedRow { get; set; }

        /// <summary>
        /// Точность вычислений.
        /// </summary>
        public float Precision { get; set; } = 0.001F;

        /// <summary>
        /// Вычесть из одной строки другую.
        /// </summary>
        /// <param name="startIndex">Стартовый индекс.</param>
        public void SubstractRows(int startIndex)
        {
            for (int i = 0; i < Rows.Values.Count; i++)
            {
                if (Rows.Keys.ElementAt(i) > startIndex)
                {
                    var row = Rows.Values.ElementAt(i);
                    if (row[startIndex] != 0)
                    {
                        var multiplier = row[startIndex] / SubstractedRow[startIndex];
                        for (int j = 0; j < row.Length; j++)
                        {
                            var removeValue = SubstractedRow[j] * multiplier;
                            row[j] -= removeValue;
                            if (System.Math.Round(row[j], 6) == 0)
                            {
                                row[j] = 0;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Считает конечное значение Х для строки.
        /// </summary>
        /// <param name="intermediateXNumber">Промежуточное значение Х.</param>
        /// <param name="matrixNumber">Множитель перед промежуточным значением Х.</param>
        /// <returns>Конечный Х.</returns>
        public static float CalculateX(float intermediateXNumber, float matrixNumber)
        {
            if (matrixNumber == 0)
            {
                return 0;
            }

            return intermediateXNumber / matrixNumber;
        }

        /// <summary>
        /// Считает промежуточное значение Х.
        /// </summary>
        /// <param name="intermediateXNumber">Предыдущее промежуточное значение Х в этой строке.</param>
        /// <param name="currentX">Полученный конечный Х строки ниже.</param>
        /// <param name="matrixNumber">Множитель перед промежуточном Х.</param>
        /// <returns>Промежуточный Х.</returns>
        public static float CalculateIntermediateX(float intermediateXNumber, float currentX, float matrixNumber)
        {
            return intermediateXNumber - matrixNumber * currentX;
        }
    }
}
