using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Common.Models.Dto;
using Common.Models.Filters;
using Common.Repositories.Contract;
using Common.Services.Contract;

namespace Common.Services
{
    /// <summary>
    /// Базовый сервис чтения данных
    /// </summary>
    /// <typeparam name="TEntity">Сущность контекста</typeparam>
    /// <typeparam name="TFilter">Фильтр сущностей</typeparam>
    /// <typeparam name="TReadRepository"></typeparam>
    public class ReadService<TEntity, TFilter, TReadRepository> : IReadService<TEntity, TFilter>
        where TEntity : class
        where TFilter : BaseFilter
        where TReadRepository : IReadRepository<TEntity, TFilter>
    {
        protected readonly Lazy<TReadRepository> repository;

        protected ReadService(Lazy<TReadRepository> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Получить сущность по идентификационному номеру
        /// </summary>
        /// <param name="key">Ключ cущности</param>
        /// <returns>Сущность</returns>
        public virtual TEntity Get(params object[] key)
        {
            return repository.Value.Get(key);
        }

        /// <summary>
        /// Асинхронно получить сущность по идентификационному номеру
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>Сущность</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<TEntity> GetAsync(params object[] key)
        {
            return Task.FromResult(Get(key));
        }

        /// <summary>
        /// Получить страницу сущностей
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Страница сущностей</returns>
        public virtual ResultDto<TEntity> Get(TFilter filter)
        {
            return repository.Value.Get(filter);
        }

        /// <summary>
        /// Асинхронно получить страницу сущностей
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Страница сущностей</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<ResultDto<TEntity>> GetAsync(TFilter filter)
        {
            return Task.FromResult(Get(filter));
        }

        /// <summary>
        /// Получить все сущности
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Все сущности</returns>
        public virtual IEnumerable<TEntity> GetAll(TFilter filter)
        {
            return repository.Value.GetAll(filter);
        }

        /// <summary>
        /// Асинхронно получить все сущности
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Все сущности</returns>
        public Task<IEnumerable<TEntity>> GetAllAsync(TFilter filter)
        {
            return Task.FromResult(GetAll(filter));
        }

        /// <summary>
        /// Проверить существуют ли сущности
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Логическое значение</returns>
        public virtual bool Exist(TFilter filter)
        {
            return repository.Value.Exist(filter);
        }

        /// <summary>
        /// Асинхронно проверить существуют ли сущности
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Логическое значение</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<bool> ExistAsync(TFilter filter)
        {
            return Task.FromResult(repository.Value.Exist(filter));
        }

        /// <summary>
        /// Получить количество сущностей
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Количество сущностей</returns>
        public virtual long Count(TFilter filter)
        {
            return repository.Value.Count(filter);
        }

        /// <summary>
        /// Асинхронно получить количество сущностей
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Количество сущностей</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<long> CountAsync(TFilter filter)
        {
            return Task.FromResult(Count(filter));
        }
    }
}