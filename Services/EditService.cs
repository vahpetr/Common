using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Common.Repositories.Contract;
using Common.Services.Contract;

namespace Common.Services
{
    /// <summary>
    /// Базовый сервис редактирования данных
    /// </summary>
    /// <typeparam name="TEntity">Тип сущьности</typeparam>
    /// <typeparam name="TEditRepository">Тип хранилища редактирования</typeparam>
    public class EditService<TEntity, TEditRepository> : IEditService<TEntity>
        where TEntity : class
        where TEditRepository : IEditRepository<TEntity>
    {
        protected readonly Lazy<TEditRepository> repository;

        public EditService(Lazy<TEditRepository> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Добавить граф сущности
        /// </summary>
        /// <param name="entity">Новаый граф сущности</param>
        public virtual void Add(TEntity entity)
        {
            repository.Value.Add(entity);
        }

        /// <summary>
        /// Асинхронно добавить граф сущности
        /// </summary>
        /// <param name="entity">Новаый граф сущности</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task AddAsync(TEntity entity)
        {
            return Task.Run(() => Add(entity));
        }

        /// <summary>
        /// Обновить граф сущности
        /// </summary>
        /// <param name="currEntity">Обновляемая сущность</param>
        /// <param name="prevEntity">Сущность из базы данных</param>
        public virtual void Update(TEntity currEntity, TEntity prevEntity)
        {
            repository.Value.Update(currEntity, prevEntity);
        }

        /// <summary>
        /// Асинхронно обновить граф сущности
        /// </summary>
        /// <param name="currEntity">Обновляемая сущность</param>
        /// <param name="prevEntity">Сущность из базы данных</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task UpdateAsync(TEntity currEntity, TEntity prevEntity)
        {
            return Task.Run(() => Update(currEntity, prevEntity));
        }

        /// <summary>
        /// Удалить сущность
        /// </summary>
        /// <param name="entity">Сущность</param>
        public virtual void Remove(TEntity entity)
        {
            repository.Value.Remove(entity);
        }

        /// <summary>
        /// Асинхронно удалить сущность
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task RemoveAsync(TEntity entity)
        {
            return Task.Run(() => Remove(entity));
        }

        /// <summary>
        /// Сохранить все изменения
        /// </summary>
        /// <returns>Количество изменённых строк в базе</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual int Commit()
        {
            return repository.Value.SaveChanges();
        }

        /// <summary>
        /// Асинхронно сохранить все изменения
        /// </summary>
        /// <returns>Количество изменённых строк в базе</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<int> CommitAsync()
        {
            return repository.Value.SaveChangesAsync();
        }
    }
}