﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Repositories.Contract
{
    /// <summary>
    /// Интерфейс храналища редактирования данных
    /// </summary>
    /// <typeparam name="TEntity">Тип сущьностит </typeparam>
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
        /// <param name="entity">Сущность</param>
        /// <param name="expressions">Свойства</param>
        void Modified(TEntity entity, params Expression<Func<TEntity, object>>[] expressions);

        /// <summary>
        /// Обновить граф сущности
        /// </summary>
        /// <param name="currEntity">Обновляемый граф сущности</param>
        /// <param name="prevEntity">Текущая сущьность</param>
        void Update(TEntity currEntity, TEntity prevEntity);

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
        /// Удалить сущность
        /// </summary>
        /// <param name="key">Ключ</param>
        void Remove(params object[] key);

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