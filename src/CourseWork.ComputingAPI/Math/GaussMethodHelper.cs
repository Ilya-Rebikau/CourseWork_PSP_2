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
        /// <param name="startIndex"></param>
        public void SubstractRows(int startIndex)
        {
            for (int i = 0; i < Rows.Values.Count; i++)
            {
                if (Rows.Keys.ElementAt(i) > startIndex)
                {
                    var indexOfFirstNotZero = FirstNotZero(Rows.Values.ElementAt(i));
                    var valueOfFirstNotZero = Rows.Values.ElementAt(i)[indexOfFirstNotZero];
                    var secondValue = SubstractedRow[indexOfFirstNotZero];
                    for (int j = 0; j < Rows.Values.ElementAt(i).Length; j++)
                    {
                        var currentValue = Rows.Values.ElementAt(i)[j];
                        var removeValue = SubstractedRow[j] * (valueOfFirstNotZero / secondValue);
                        if (currentValue - removeValue > 0 &&
                            currentValue - removeValue < Precision)
                        {
                            Rows.Values.ElementAt(i)[j] = 0;
                        }
                        else
                        {
                            Rows.Values.ElementAt(i)[j] -= removeValue;
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
        /// <returns></returns>
        public static float CalculateX(float intermediateXNumber, float matrixNumber)
        {
            return intermediateXNumber / matrixNumber;
        }

        /// <summary>
        /// Считает промежуточное значение Х.
        /// </summary>
        /// <param name="intermediateXNumber">Предыдущее промежуточное значение Х в этой строке.</param>
        /// <param name="currentX">Полученный конечный Х строки ниже.</param>
        /// <param name="matrixNumber">Множитель перед промежуточном Х.</param>
        /// <returns></returns>
        public static float CalculateIntermediateX(float intermediateXNumber, float currentX, float matrixNumber)
        {
            return intermediateXNumber - matrixNumber * currentX;
        }

        /// <summary>
        /// Получает первый ненулевой элемент строки.
        /// </summary>
        /// <param name="stroka"></param>
        /// <returns></returns>
        private static int FirstNotZero(float[] row)
        {
            var notZero = row.First(x => x != 0);
            return row.ToList().IndexOf(notZero);
        }
    }
}
