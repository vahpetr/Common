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
        private static Expression<Func<TEntity, bool>> GetExpression<TEntity>(
           params object[] key)
           where TEntity : class
        {
            var keyProps = EntityUtilites<TEntity>.KeyProps;
            if (key.Count() != keyProps.Count()) return null;

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
                object[] args = { key[i++], null };
                var result = (bool)tryConvertValue.Invoke(null, args);
                if (!result) return null;
                var constant = Expression.Constant(args[1]);
                var equal = Expression.Equal(property, constant);
                expressions.Add(equal);
            }

            var body = expressions.Aggregate(Expression.And);
            var expression = Expression.Lambda<Func<TEntity, bool>>(body, entityParameter);

            return expression;
        }

        public static TEntity Find<TEntity>(
           this IQueryable<TEntity> source,
           params object[] key)
           where TEntity : class
        {
            var expression = GetExpression<TEntity>(key);

            var call = Expression.Call(
                typeof(Queryable),
                "FirstOrDefault",
                new[] { source.ElementType },
                source.Expression,
                Expression.Quote(expression)
                );

            return source.Provider.Execute<TEntity>(call);
        }

        public static bool Exist<TEntity>(
           this IQueryable<TEntity> source,
           params object[] key)
           where TEntity : class
        {
            var expression = GetExpression<TEntity>(key);

            var call = Expression.Call(
                typeof(Queryable),
                "Any",
                new[] { source.ElementType },
                source.Expression,
                Expression.Quote(expression)
                );

            return source.Provider.Execute<bool>(call);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FillKey<TEntity>(this TEntity entity, params object[] key) where TEntity : class
        {
            EntityFillKeyUtilite<TEntity>.Mapper(key, entity);
        }

        public static TEntity Find<TEntity>(
            this IEnumerable<TEntity> source,
            params object[] key)
            where TEntity : class
        {
            var keyProps = EntityUtilites<TEntity>.KeyProps;
            if (key.Count() != keyProps.Count()) return null;
            var keyEqual = EntityKeyEqualUtilites<TEntity>.KeyEqual;
            return source.FirstOrDefault(p => keyEqual(p, key));
        }

        public static bool Exist<TEntity>(
            this IEnumerable<TEntity> source,
            params object[] key)
            where TEntity : class
        {
            var keyProps = EntityUtilites<TEntity>.KeyProps;
            if (key.Count() != keyProps.Count()) return false;
            var keyEqual = EntityKeyEqualUtilites<TEntity>.KeyEqual;
            return source.Any(p => keyEqual(p, key));
        }
    }
}