﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Utilites
{
    public static class EntityUtilites<TEntity> where TEntity : class
    {
        public static readonly PropertyInfo[] KeyProps;

        static EntityUtilites()
        {
            var type = typeof(TEntity);
            var props = typeof (TEntity).GetProperties().Where(p => p.CanRead);
            var keys = props.Where(p => p.GetCustomAttributes(typeof (KeyAttribute), true).Any());

            if (!keys.Any())
            {
                var metadataType = type.GetCustomAttributes(typeof(MetadataTypeAttribute), true).OfType<MetadataTypeAttribute>().FirstOrDefault();

                if (metadataType != null)
                {
                    keys = metadataType.MetadataClassType.GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Any());
                }
                
            }

            var list = new List<Tuple<int, PropertyInfo>>(4);

            foreach (var key in keys)
            {
                var attribute = Attribute.GetCustomAttribute(key, typeof (ColumnAttribute)) as ColumnAttribute;
                list.Add(new Tuple<int, PropertyInfo>(attribute == null ? 0 : attribute.Order, key));
            }

            if (list.Any())
            {
                KeyProps = list.OrderBy(p => p.Item1).Select(p => p.Item2).ToArray();
            }
            else
            {
                var prop = props.FirstOrDefault(p => p.Name.ToLower() == "id");

                if (prop != null)
                {
                    KeyProps = new[] {prop};
                }
                else
                {
                    throw new Exception("Объекты без ключа не поддерживаются");
                }
            }
        }

        public static PropertyInfo[] Get()
        {
            return KeyProps;
        }
    }

    public static class EntityGetKeyExpressionUtilites<TEntity> where TEntity : class
    {
        public static readonly Expression<Func<TEntity, object[]>> GetKeyExpression;
        public static readonly Expression<Func<TEntity, TEntity>> IdentityExpression;

        static EntityGetKeyExpressionUtilites()
        {
            var entityParameter = Expression.Parameter(typeof (TEntity), "p");
            var expressions = new List<Expression>();

            foreach (var part in EntityUtilites<TEntity>.KeyProps)
            {
                var property = Expression.Property(entityParameter, part.Name);

                var type = typeof (object);

                if (property.Type.IsEnum)
                {
                    //иначе Enum превратится в своё строкове название
                    type = Enum.GetUnderlyingType(property.Type);
                    var valueType = Expression.Convert(property, type);
                    var value = Expression.Convert(valueType, typeof (object));
                    expressions.Add(value);
                }
                else
                {
                    var convert = Expression.Convert(property, type);
                    expressions.Add(convert);
                }
            }

            var body = Expression.NewArrayInit(typeof (object), expressions);

            GetKeyExpression = Expression.Lambda<Func<TEntity, object[]>>(body, entityParameter);
        }
    }

    public static class EntityIdentityExpressionUtilites<TEntity> where TEntity : class
    {
        public static readonly Expression<Func<TEntity, TEntity>> IdentityExpression;

        static EntityIdentityExpressionUtilites()
        {
            var type = typeof (TEntity);
            var parameter = Expression.Parameter(type);
            var bindings = new List<MemberAssignment>();

            foreach (var prop in EntityUtilites<TEntity>.KeyProps)
            {
                bindings.Add(Expression.Bind(prop, Expression.Property(parameter, prop)));
            }

            IdentityExpression =
                Expression.Lambda<Func<TEntity, TEntity>>(Expression.MemberInit(Expression.New(type), bindings),
                    parameter);
        }
    }

    public static class EntityGetKeyUtilites<TEntity> where TEntity : class
    {
        public static readonly Func<TEntity, object[]> GetKey;

        static EntityGetKeyUtilites()
        {
            GetKey = EntityGetKeyExpressionUtilites<TEntity>.GetKeyExpression.Compile();
        }

        public static object[] Get(TEntity entity)
        {
            return GetKey(entity);
        }
    }

    public static class EntityIdentityUtilites<TEntity> where TEntity : class
    {
        public static readonly Func<TEntity, TEntity> Identity;

        static EntityIdentityUtilites()
        {
            Identity = EntityIdentityExpressionUtilites<TEntity>.IdentityExpression.Compile();
        }
    }

    public static class EntityKeyEqualExpressionUtilites<TEntity>
        where TEntity : class
    {
        public static readonly Expression<Func<TEntity, object[], bool>> KeyEqualExpression;

        static EntityKeyEqualExpressionUtilites()
        {
            var entityType = typeof(TEntity);
            var keys = EntityUtilites<TEntity>.KeyProps;
            var entityParameter = Expression.Parameter(entityType, "p");
            var expressions = new List<BinaryExpression>();
            var keysParameter = Expression.Parameter(typeof(object[]), "keys");

            var i = 0;
            foreach (var part in keys)
            {
                var property = Expression.Property(entityParameter, part.Name);
                var arrayAccess = Expression.ArrayAccess(keysParameter, Expression.Constant(i++));
                var methodToString = arrayAccess.Type.GetMethod("ToString", Type.EmptyTypes);
                var toString = Expression.Call(arrayAccess, methodToString);

                MethodInfo methodParse;
                MethodCallExpression parse;
                UnaryExpression value;

                if (property.Type == typeof(string))
                {
                    value = Expression.Convert(toString, property.Type);
                }
                else if (property.Type.IsEnum)
                {
                    var genericType = typeof(TypeUtilites<>).MakeGenericType(property.Type);
                    var tryConvertValue = genericType.GetMethod("ParseEnum", new[] { property.Type });
                    var result = Expression.Call(tryConvertValue, new Expression[] { arrayAccess });
                    value = Expression.Convert(result, property.Type);
                }
                else
                {
                    methodParse = property.Type.GetMethod("Parse", new[] { typeof(string) });
                    parse = Expression.Call(methodParse, toString);
                    value = Expression.Convert(parse, property.Type);
                }

                var expression = Expression.Equal(property, value);
                expressions.Add(expression);
            }

            var body = expressions.Aggregate(Expression.AndAlso);

            KeyEqualExpression = Expression.Lambda<Func<TEntity, object[], bool>>(body, entityParameter, keysParameter);
        }
    }

    public static class EntityKeyEqualUtilites<TEntity>
        where TEntity : class
    {
        public static readonly Func<TEntity, object[], bool> KeyEqual;

        static EntityKeyEqualUtilites()
        {
            KeyEqual = EntityKeyEqualExpressionUtilites<TEntity>.KeyEqualExpression.Compile();
        }
    }

    public static class EntityFillKeyExpressionUtilite<TTo> where TTo : class
    {
        public static readonly Expression<Action<object[], TTo>> Mapper;

        static EntityFillKeyExpressionUtilite()
        {
            var type = typeof(TTo);
            var entity = Expression.Parameter(type, "entity");
            var array = Expression.Parameter(typeof(object[]), "key");
            var expressions = new List<Expression>();
            var props = EntityUtilites<TTo>.KeyProps;

            var i = 0;
            foreach (var prop in props)
            {
                var value = Expression.ArrayIndex(array, Expression.Constant(i++));
                var convert = Expression.Convert(value, prop.PropertyType);
                var field = Expression.Property(entity, prop);
                var assing = Expression.Assign(field, convert);
                expressions.Add(assing);
            }

            var block = Expression.Block(expressions);

            Mapper = Expression.Lambda<Action<object[], TTo>>(block, new[] { array, entity });
        }
    }

    public static class EntityFillKeyUtilite<TTo> where TTo : class
    {
        public static readonly Action<object[], TTo> Mapper;

        static EntityFillKeyUtilite()
        {
            Mapper = EntityFillKeyExpressionUtilite<TTo>.Mapper.Compile();
        }
    }

    public static class EntityInitExpressionUtilite<TTo> where TTo : class
    {
        public static readonly Expression<Func<object[], TTo>> Mapper;

        static EntityInitExpressionUtilite()
        {
            var type = typeof(TTo);
            var array = Expression.Parameter(typeof(object[]));
            var bindings = new List<MemberBinding>();
            var props = EntityUtilites<TTo>.KeyProps;

            var i = 0;
            foreach (var prop in props)
            {
                var body = Expression.ArrayIndex(array, Expression.Constant(i++));
                var convert = Expression.Convert(body, prop.PropertyType);
                bindings.Add(Expression.Bind(prop, convert));
            }

            Mapper = Expression.Lambda<Func<object[], TTo>>(Expression.MemberInit(Expression.New(type), bindings),
                new[] {array});
        }
    }

    public static class EntityInitUtilite<TTo> where TTo : class
    {
        public static readonly Func<object[], TTo> Mapper;

        static EntityInitUtilite()
        {
            Mapper = EntityInitExpressionUtilite<TTo>.Mapper.Compile();
        }
    }
}