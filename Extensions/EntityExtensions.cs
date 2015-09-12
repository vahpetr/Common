using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Common.Utilites;

namespace Common.Extensions
{
    public static class EntityExtensions
    {
        public static TEntity Find<TEntity>(
           this IQueryable<TEntity> source,
           params object[] keys)
           where TEntity : class
        {
            var keyProps = EntityUtilites<TEntity>.KeyProps;
            if (keys.Count() != keyProps.Count()) return null;

            var entityType = typeof(TEntity);
            var entityParameter = Expression.Parameter(entityType, "p");
            var expressions = new List<BinaryExpression>();

            var i = 0;
            foreach (var part in keyProps)
            {
                var property = Expression.Property(entityParameter, part.Name);
                var genericType = typeof(TypeUtilites<>).MakeGenericType(property.Type);
                var tryConvertValue = genericType.GetMethod("TryConvertValue",
                    new[] { typeof(object), property.Type.MakeByRefType() });
                object[] args = { keys[i++], null };
                var result = (bool)tryConvertValue.Invoke(null, args);
                if (!result) return null;
                var constant = Expression.Constant(args[1]);
                var equal = Expression.Equal(property, constant);
                expressions.Add(equal);
            }

            var body = expressions.Aggregate(Expression.And);
            var expression = Expression.Lambda<Func<TEntity, bool>>(body, entityParameter);

            var call = Expression.Call(
                typeof(Queryable),
                "FirstOrDefault",
                new[] { entityType },
                source.Expression,
                Expression.Quote(expression)
                );

            return source.Provider.Execute<TEntity>(call);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object[] GetKey<TEntity>(this TEntity entity)
            where TEntity : class
        {
            return EntityGetKeyUtilites<TEntity>.GetKey(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TEntity Identity<TEntity>(this TEntity entity) where TEntity : class
        {
            return EntityIdentityUtilites<TEntity>.Identity(entity);
        }

        public static TEntity Find<TEntity>(
            this IEnumerable<TEntity> source,
            params object[] keys)
            where TEntity : class
        {
            var keyProps = EntityUtilites<TEntity>.KeyProps;
            if (keys.Count() != keyProps.Count()) return null;
            var keyEqual = EntityKeyEqualUtilites<TEntity>.KeyEqual;
            return source.FirstOrDefault(p => keyEqual(p, keys));
        }
    }
}