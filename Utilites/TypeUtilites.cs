using System;

namespace Common.Utilites
{
    public static class TypeUtilites<T>
    {
        public static bool TryConvertValue(object value, out T obj)
        {
            var type = typeof(T);
            var underlyingType = ObjectUtilites.GetUnderlyingType(type);

            if (!type.IsEnum && underlyingType == value.GetType())
            {
                //TODO как по другому?
                obj = (T)Convert.ChangeType(value, type);
                return true;
            }

            var method = underlyingType.GetMethod("TryParse", new[] { typeof(string), underlyingType.MakeByRefType() });

            if (method == null)
            {
                obj = default(T);
                return false;
            }

            object[] args = { value.ToString(), null };
            var result = (bool)method.Invoke(null, args);
            if (!result)
            {
                obj = default(T);
                return false;
            }

            obj = (T)args[1];
            return true;
        }
    }
}