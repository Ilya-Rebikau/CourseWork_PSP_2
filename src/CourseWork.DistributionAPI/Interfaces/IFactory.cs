namespace CourseWork.DistributionAPI.Interfaces
{
    /// <summary>
    /// Интерфейс фабричного метода.
    /// </summary>
    /// <typeparam name="T">Тип создаваемой модели.</typeparam>
    public interface IFactory<T>
    {
        /// <summary>
        /// Создаёт лист моделей.
        /// </summary>
        /// <returns>Лист созданных моделей.</returns>
        public List<T> CreateList();
    }
}
