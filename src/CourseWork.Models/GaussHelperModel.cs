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
        /// Вычитаемая строка.
        /// </summary>
        public float[] SubstractedRow { get; set; }

        /// <summary>
        /// Строки с их индексами, из которых необходимо вычесть вычитаемую строку.
        /// </summary>
        public Dictionary<int, float[]> Rows { get; set; }
    }
}
