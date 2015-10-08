using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Utilites
{
    /// <summary>
    /// Утилиты деревьев выражений
    /// </summary>
    public static class ExpressionUtilites
    {
        /// <summary>
        /// Получить выражение доступа к свойству объекта
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="expression">Выражение</param>
        /// <returns>Выражение доступа к свойству объекта</returns>
        public static MemberExpression GetMemberExpression<T>(Expression<Func<T, object>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member != null) return member;
            var unary = expression.Body as UnaryExpression;
            if (unary == null) return null;
            return unary.Operand as MemberExpression;
        }

        /// <summary>
        /// Получить свойство объекта
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="expression">Выражение</param>
        /// <returns>Свойство объекта</returns>
        public static PropertyInfo GetProperty<T>(Expression<Func<T, object>> expression)
        {
            var memberExpression = GetMemberExpression(expression);
            var property = memberExpression.Member as PropertyInfo;
            if (property != null) return property;
            return null;
        }

        /// <summary>
        /// Получить значение свойства объекта
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="expression">Выражение</param>
        /// <param name="obj">Объект</param>
        /// <returns>Значение свойства объекта</returns>
        public static object GetValue<T>(Expression<Func<T, object>> expression, T obj)
        {
            var property = GetProperty(expression);
            if (property == null) return null;
            var value = property.GetValue(obj);
            return value;
        }

        /// <summary>
        /// Изменить значение свойства объекта
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="expression">Выражение</param>
        /// <param name="obj">Объект</param>
        /// <param name="value">Новое значение</param>
        public static void SetValue<T>(Expression<Func<T, object>> expression, T obj, object value)
        {
            var property = GetProperty(expression);
            if (property == null) return;
            property.SetValue(obj, value, null);
        }
    }
}