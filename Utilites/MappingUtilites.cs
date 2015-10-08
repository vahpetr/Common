using Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Common.Utilites
{
    public static class MappingExpressionUtilites<TFrom, TTo> where TFrom : class where TTo : class
    {
        public static readonly Expression<Func<TFrom, TTo>> Mapper;

        static MappingExpressionUtilites()
        {
            //MapperExpression
            var from = typeof(TFrom);
            var to = typeof(TTo);
            var fromProps = from.GetProperties().Where(p => p.CanRead);
            var toProps = to.GetProperties().Where(p => p.CanWrite);
            var parameter = Expression.Parameter(from);
            var bindings = new List<MemberAssignment>();

            foreach (var toProp in toProps)
            {
                foreach (var fromProp in fromProps)
                {
                    if (fromProp.Name != toProp.Name) continue;
                    bindings.Add(Expression.Bind(toProp, Expression.Property(parameter, fromProp)));
                    break;
                }
            }
            Mapper = Expression.Lambda<Func<TFrom, TTo>>(Expression.MemberInit(Expression.New(to), bindings), parameter);
        }
    }

    public static class MappingUtilites<TFrom, TTo>
        where TFrom : class
        where TTo : class
    {
        public static readonly Func<TFrom, TTo> Mapper;

        static MappingUtilites()
        {
            Mapper = MappingExpressionUtilites<TFrom, TTo>.Mapper.Compile();
        }
    }

    public static class MappingExpressionUtilites<TTo> where TTo : class
    {
        public static readonly Expression<Func<Dictionary<string, object>, TTo>> Mapper;

        static MappingExpressionUtilites()
        {
            var type = typeof(TTo);
            var parameter = Expression.Parameter(typeof(Dictionary<string, object>));
            var bindings = new List<MemberBinding>();
            var props = type.GetProperties().Where(p => p.CanWrite);

            foreach (var prop in props)
            {
                var call = Expression.Call(
                    typeof(DictionaryExtensions),
                    "GetValue",
                    new[] { prop.PropertyType },
                    new Expression[] { parameter, Expression.Constant(prop.Name) }
                    );
                bindings.Add(Expression.Bind(prop, call));
            }

            Mapper = Expression.Lambda<Func<Dictionary<string, object>, TTo>>(
                    Expression.MemberInit(Expression.New(type), bindings), new[] { parameter });
        }
    }

    public static class MappingUtilites<TTo> where TTo : class
    {
        public static readonly Func<Dictionary<string, object>, TTo> Mapper;

        static MappingUtilites()
        {
            Mapper = MappingExpressionUtilites<TTo>.Mapper.Compile();
        }
    }
}