using System;

namespace Common.Models.Filters
{
    /// <summary>
    /// Пэйджинг
    /// </summary>
    public class PagingFilter
    {
        public PagingFilter()
        {
            Count = 20;
            Page = 1;
        }

        private int count;
        private int page;

        /// <summary>
        /// Всего страниц
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Количество записей на странице
        /// </summary>
        public int Count
        {
            get { return count; }
            set { count = value < 1 ? 1 : (value > 100 ? 100 : value); }
        }

        /// <summary>
        /// Страница
        /// </summary>
        public int Page
        {
            get { return page; }
            set { page = value <= 0 ? 1 : value; }
        }

        /// <summary>
        /// Страниц
        /// </summary>
        public int Pages
        {
            get
            {
                return (int) Math.Ceiling(Total/Count);
            }
        }


        /// <summary>
        /// Индекс страници
        /// </summary>
        public int Index
        {
            get
            {
                return Page - 1;
            }
        }

        /// <summary>
        /// Сколько элементов пропустить
        /// </summary>
        public int Skip {
            get
            {
                return Index * Count;
            }
        }

        /// <summary>
        /// Сколько элементов получить
        /// </summary>
        public int Take
        {
            get
            {
                return Count;
            }
        }
    }
}