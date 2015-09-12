using System.Collections.Generic;

namespace Common.Models.Dto
{
    /// <summary>
    /// Результат
    /// </summary>
    /// <typeparam name="TEntity">Сущьность</typeparam>
    public class ResultDto<TEntity> where TEntity : class
    {
        /// <summary>
        /// Общее количество
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// Данные
        /// </summary>
        public IEnumerable<TEntity> Data { get; set; }
    }
}