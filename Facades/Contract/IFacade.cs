using System.Threading.Tasks;
using Common.Dto;
using Common.Filters;

namespace Common.Facades.Contract
{
    public interface IFacade<TEntity, in TFilter>
        where TEntity : class
        where TFilter : BaseFilter
    {
        /// <summary>
        /// Асинхронно получить сущность по номеру
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>Страница сущностей</returns>
        Task<TEntity> Get(params object[] key);

        /// <summary>
        /// Асинхронно получить данные
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Данные</returns>
        Task<ResultDto<TEntity>> Get(TFilter filter);

        /// <summary>
        /// Асинхронно проверить существуют ли сущности
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Логическое значение</returns>
        Task<bool> Exist(TFilter filter);

        /// <summary>
        /// Асинхронно получить количество сущностей
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Количество сущностей</returns>
        Task<long> Count(TFilter filter);

        /// <summary>
        /// Асинхронно добавить граф сущности
        /// </summary>
        /// <param name="entity">Новаый граф сущности</param>
        /// <returns></returns>
        Task Add(TEntity entity);

        /// <summary>
        /// Асинхронно обновить граф сущности
        /// </summary>
        /// <param name="currEntity">Обновляемая сущность</param>
        /// <param name="prevEntity">Сущность из базы данных</param>
        /// <returns></returns>
        Task Update(TEntity currEntity, TEntity prevEntity);

        /// <summary>
        /// Асинхронно удалить сущность
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <returns></returns>
        Task Remove(TEntity entity);
    }
}