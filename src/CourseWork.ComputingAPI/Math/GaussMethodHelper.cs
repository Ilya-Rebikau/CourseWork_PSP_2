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
    }
}
