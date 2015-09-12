using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Repositories.Contract
{
    /// <summary>
    /// Интерфейс храналища редактирования данных
    /// </summary>
    /// <typeparam name="TEntity">Сущьность</typeparam>
    public interface IEditRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Добавить граф сущности
        /// </summary>
        /// <param name="entity">Новаый граф сущности</param>
        void Add(TEntity entity);

        /// <summary>
        /// Пометить свойство сущьности как изменённое
        /// </summary>
        /// <typeparam name="TProperty">Тип свойства сущности</typeparam>
        /// <param name="entity">Сущность</param>
        /// <param name="property">Свойство</param>
        void Modified<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property);

        /// <summary>
        /// Обновить граф сущности
        /// </summary>
        /// <param name="entity">Обновляемый граф сущности</param>
        void Update(TEntity entity);

        /// <summary>
        /// Удалить сущность
        /// </summary>
        /// <param name="entity">Сущность</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Сохранить все изменения
        /// </summary>
        /// <returns>Количество изменённых строк в базе</returns>
        int SaveChanges();

        /// <summary>
        /// Асинхронно сохранить все изменения
        /// </summary>
        /// <returns>Количество изменённых строк в базе</returns>
        Task<int> SaveChangesAsync();
    }
}