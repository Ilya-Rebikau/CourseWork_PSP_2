using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CourseWork.UnitTests")]

namespace CourseWork.Web.Interfaces
{
    using CourseWork.Models;

    /// <summary>
    /// Интерфейс сервиса для сериализация и десериализации.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Десериализует матрицу из файла.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <returns>Десериализованная матрица.</returns>
        Task<Matrix> DeserializeMatrix(string path);

        /// <summary>
        /// Десериализует вектор из файла.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <returns>Десериализованный вектор.</returns>
        Task<Vector> DeserializeVector(string path);

        /// <summary>
        /// Сериализует вектор.
        /// </summary>
        /// <param name="vector">Вектор для десериализации.</param>
        /// <returns>Массив байтов вектора.</returns>
        byte[] SerializeVector(Vector vector);
    }
}
