using System.Threading.Tasks;

namespace Common.Services.Contract
{
    /// <summary>
    /// Интерфейс сервиса редактирования данных
    /// </summary>
    /// <typeparam name="TEntity">Сущьность</typeparam>
    public interface IEditService<in TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Добавить граф сущности
        /// </summary>
        /// <param name="entity">Новаый граф сущности</param>
        void Add(TEntity entity);

        /// <summary>
        /// Асинхронно добавить граф сущности
        /// </summary>
        /// <param name="entity">Новаый граф сущности</param>
        /// <returns></returns>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Обновить граф сущности
        /// </summary>
        /// <param name="currEntity">Обновляемая сущность</param>
        /// <param name="prevEntity">Сущность из базы данных</param>
        void Update(TEntity currEntity, TEntity prevEntity);

        /// <summary>
        /// Асинхронно обновить граф сущности
        /// </summary>
        /// <param name="currEntity">Обновляемая сущность</param>
        /// <param name="prevEntity">Сущность из базы данных</param>
        /// <returns></returns>
        Task UpdateAsync(TEntity currEntity, TEntity prevEntity);

        /// <summary>
        /// Удалить сущность
        /// </summary>
        /// <param name="entity">Сущность</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Асинхронно удалить сущность
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <returns></returns>
        Task RemoveAsync(TEntity entity);

        /// <summary>
        /// Сохранить все изменения
        /// </summary>
        /// <returns>Количество изменённых строк в базе</returns>
        int Commit();

        /// <summary>
        /// Асинхронно сохранить все изменения
        /// </summary>
        /// <returns>Количество изменённых строк в базе</returns>
        Task<int> CommitAsync();
    }
}