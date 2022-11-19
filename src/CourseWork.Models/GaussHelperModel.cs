namespace CourseWork.Models
{
    /// <summary>
    /// Вспомогательная модель для метода Гаусса.
    /// </summary>
    public class GaussHelperModel
    {
        /// <summary>
        /// Индекс с которого начать вычитание строк.
        /// </summary>
        public int StartIndex { get; set; }
        /// <summary>
        /// Промежуточное значение Х.
        /// </summary>
        public float IntermediateX { get; set; }

        /// <summary>
        /// Конечный вычисленный Х для строки.
        /// </summary>
        public float CurrentX { get; set; }

        /// <summary>
        /// Множитель перед Х.
        /// </summary>
        public float MatrixNumber { get; set; }

        /// <summary>
        /// Вычитаемая строка.
        /// </summary>
        public float[] SubrstractedRow { get; set; }

        /// <summary>
        /// Строки с их индексами, из которых необходимо вычесть вычитаемую строку.
        /// </summary>
        public Dictionary<int, float[]> Rows { get; set; }
    }
}
