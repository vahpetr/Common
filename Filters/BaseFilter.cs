using Common.Enums;

namespace Common.Filters
{
    /// <summary>
    /// Базовый фильтр
    /// </summary>
    public class BaseFilter : PagingFilter
    {
        private string _sortBy;

        public BaseFilter()
        {
            SortBy = "Position";
            Order = Order.Desc;
        }

        /// <summary>
        /// Сортировать по
        /// </summary>
        public string SortBy
        {
            get
            {
                return _sortBy;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _sortBy = "Position";
                    return;
                }

                //http://www.dotnetperls.com/uppercase-first-letter
                var array = value.ToCharArray();
                // Handle the first letter in the string.
                if (array.Length >= 1)
                {
                    if (char.IsLower(array[0]))
                    {
                        array[0] = char.ToUpper(array[0]);
                    }
                }
                // Scan through the letters, checking for spaces.
                // ... Uppercase the lowercase letters following spaces.
                for (var i = 1; i < array.Length; i++)
                {
                    if (array[i - 1] != '.') continue;
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
                _sortBy = new string(array);
            }
        }

        /// <summary>
        /// Направление сортировки (asc, desc)
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// Поиск по тексту
        /// </summary>
        public string Q { get; set; }
    }
}