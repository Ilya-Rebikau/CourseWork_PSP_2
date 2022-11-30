namespace CourseWork.ComputingAPI.Interfaces
{
    /// <summary>
    /// Помощник метода Гаусса.
    /// </summary>
    public interface IGaussMethodHelper
    {
        /// <summary>
        /// Строки с их индексами, из которых необходимо вычесть разрешающую строку.
        /// </summary>
        Dictionary<int, float[]> Rows { get; set; }

        /// <summary>
        /// Разрешающая строка.
        /// </summary>
        float[] SubstractedRow { get; set; }

        /// <summary>
        /// Вычесть разрешающую строку.
        /// </summary>
        /// <param name="startIndex">Стартовый индекс.</param>
        void SubstractRows(int startIndex);
    }
}
