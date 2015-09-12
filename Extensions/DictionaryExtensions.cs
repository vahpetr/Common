using System;
using System.Collections.Generic;

namespace Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static T GetValue<T>(this Dictionary<string, object> dictionary, string key)
        {
            object value;
            if (!dictionary.TryGetValue(key, out value)) return default(T);
            if (Nullable.GetUnderlyingType(typeof(T)) != null)
                return (T)Convert.ChangeType(value, Nullable.GetUnderlyingType(typeof(T)));
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}