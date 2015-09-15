using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Dto;

namespace Common.Services.Contract
{
    /// <summary>
    /// Интерфейс сервиса чтения
    /// </summary>
    /// <typeparam name="TEntity">Тип сущьности</typeparam>
    /// <typeparam name="TFilter">Тип фильтра</typeparam>
    public interface IReadService<TEntity, in TFilter>
        where TEntity : class
        where TFilter : class
    {
        /// <summary>
        /// Получить сущность по номеру
        /// </summary>
        /// <param name="key">Ключ cущности</param>
        /// <returns>Сущность</returns>
        TEntity Get(params object[] key);

        /// <summary>
        /// Асинхронно получить сущность по номеру
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>Страница сущностей</returns>
        Task<TEntity> GetAsync(params object[] key);

        /// <summary>
        /// Получить данные
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Данные</returns>
        ResultDto<TEntity> Get(TFilter filter);

        /// <summary>
        /// Асинхронно получить данные
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Данные</returns>
        Task<ResultDto<TEntity>> GetAsync(TFilter filter);

        /// <summary>
        /// Получить все сущности
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Все сущности</returns>
        IEnumerable<TEntity> GetAll(TFilter filter);

        /// <summary>
        /// Асинхронно получить все сущности
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Все сущности</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(TFilter filter);

        /// <summary>
        /// Проверить существуют ли сущности
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Логическое значение</returns>
        bool Exist(TFilter filter);

        /// <summary>
        /// Асинхронно проверить существуют ли сущности
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Логическое значение</returns>
        Task<bool> ExistAsync(TFilter filter);

        /// <summary>
        /// Получить количество сущностей
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Количество сущностей</returns>
        long Count(TFilter filter);

        /// <summary>
        /// Асинхронно получить количество сущностей
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Количество сущностей</returns>
        Task<long> CountAsync(TFilter filter);
    }
}