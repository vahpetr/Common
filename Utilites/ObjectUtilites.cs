﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Common.Utilites
{
    /// <summary>
    /// Утилиты работы с объектами
    /// </summary>
    public class ObjectUtilites
    {
        /// <summary>
        /// Список примитивных типов
        /// </summary>
        public static readonly List<Type> PrimitiveTypes = new List<Type>
        {
            typeof (byte),
            typeof (short),
            typeof (int),
            typeof (long),
            typeof(string),
            typeof (bool),
            typeof (decimal),
            typeof (float),
            typeof(Guid),
            typeof (DateTime),
            typeof(Enum),

            typeof (double),
            typeof (char),
            typeof (DateTimeOffset),
            typeof (TimeSpan)
        };

        /// <summary>
        /// Получыить основной тип
        /// </summary>
        /// <param name="type">Тип</param>
        /// <returns>Основной тип</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type GetUnderlyingType(Type type)
        {
            if (type.IsEnum) return Enum.GetUnderlyingType(type);
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        /// <summary>
        /// Это коллекция?
        /// </summary>
        /// <param name="type">Тип</param>
        /// <returns>Логическое значение</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCollection(Type type)
        {
            return type.GetInterface(typeof(IEnumerable<>).FullName) != null && type == typeof(string);
        }

        /// <summary>
        /// Получить список примитивных свойств типа
        /// </summary>
        /// <param name="type">Тип</param>
        /// <returns>Список примитивных свойств</returns>
        public static IEnumerable<PropertyInfo> GetPrimitiveProps(Type type)
        {
            return type.GetProperties()
                .Where(prop =>
                    PrimitiveTypes.Contains(GetUnderlyingType(prop.PropertyType))
                );
        }

        /// <summary>
        /// Получить список примитивных свойств типа
        /// </summary>
        /// <param name="type">Тип</param>
        /// <param name="ignoreProps">Игнорируемые типы</param>
        /// <returns>Список примитивных свойств</returns>
        public static IEnumerable<PropertyInfo> GetPrimitiveProps(Type type, params string[] ignoreProps)
        {
            return GetPrimitiveProps(type).Where(p => !ignoreProps.Contains(p.Name));
        }

        /// <summary>
        /// Получить список сложных свойства
        /// </summary>
        /// <param name="type">Тип</param>
        /// <returns>Список сложных свойств</returns>
        public static IEnumerable<PropertyInfo> GetComplexProps(Type type)
        {
            return type.GetProperties()
                .Where(prop =>
                    !PrimitiveTypes.Contains(GetUnderlyingType(prop.PropertyType)) &&
                    !IsCollection(prop.PropertyType)
                );
        }

        /// <summary>
        /// Получить список сложных свойства
        /// </summary>
        /// <param name="type">Тип</param>
        /// <param name="ignoreProps">Игнорируемые типы</param>
        /// <returns>Список сложных свойств</returns>
        public static IEnumerable<PropertyInfo> GetComplexProps(Type type, params string[] ignoreProps)
        {
            return GetComplexProps(type).Where(p => !ignoreProps.Contains(p.Name));
        }

        /// <summary>
        /// Получить список свойств коллекций
        /// </summary>
        /// <param name="type">Тип</param>
        /// <returns>Список свойств коллекций</returns>
        public static IEnumerable<PropertyInfo> GetCollectionProps(Type type)
        {
            return type.GetProperties().Where(prop => IsCollection(prop.PropertyType));
        }

        /// <summary>
        /// Получить список свойств коллекций
        /// </summary>
        /// <param name="type">Тип</param>
        /// <param name="ignoreProps">Игнорируемые типы</param>
        /// <returns>Список свойств коллекций</returns>
        public static IEnumerable<PropertyInfo> GetCollectionProps(Type type, params string[] ignoreProps)
        {
            return GetCollectionProps(type).Where(prop => !ignoreProps.Contains(prop.Name));
        }

        /// <summary>
        /// Получить редактируемые свойства
        /// </summary>
        /// <param name="type">Тип</param>
        /// <returns>Свойства существующие в базе данных</returns>
        public static IEnumerable<PropertyInfo> GetEditableProperties(Type type)
        {
            //только свойства в которые можно писать и которые можно читать есть в базе
            return GetPrimitiveProps(type).Where(p => p.CanRead && p.CanWrite);
        }

        /// <summary>
        /// Очистка свойств объекта по типу
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="target">Тип</param>
        /// <param name="references">Проверенные ссылки</param>
        public static void CleanPropertyByType(object obj, Type target, List<object> references = null)
        {
            if (references == null) references = new List<object>();

            var type = obj.GetType();

            var primitives = GetPrimitiveProps(type).Where(p => p.CanWrite && p.PropertyType == target);

            foreach (var prop in primitives)
            {
                prop.SetValue(obj, null);
            }

            var complexes = GetComplexProps(type)
                .Where(p => p.CanWrite)
                .Select(p => p.GetValue(obj))
                .Where(p => p != null && !references.Contains(p));

            foreach (var value in complexes)
            {
                references.Add(value);
                CleanPropertyByType(value, target, references);
            }

            var collections = GetCollectionProps(type).Where(p => p.CanWrite);

            foreach (var prop in collections)
            {
                if (prop.PropertyType == target)
                {
                    prop.SetValue(obj, null);
                }
                else
                {
                    var values = prop.GetValue(obj) as IEnumerable<object>;

                    if(values == null) continue;

                    foreach (var value in values.Where(p => p != null && !references.Contains(p)))
                    {
                        references.Add(value);
                        CleanPropertyByType(value, target, references);
                    }
                }
            }
        }

        /// <summary>
        /// Накатить один объект на другой
        /// </summary>
        /// <typeparam name="TFrom">Тит из которого берём значения</typeparam>
        /// <typeparam name="TTo">Тип на кторый накатываем значения</typeparam>
        /// <param name="from">Объект из которого берём значения</param>
        /// <param name="to">Объект на кторый накатываем значения</param>
        /// <returns></returns>
        public static TTo Rolled<TFrom, TTo>(TFrom from, TTo to)
        {
            var fromType = typeof(TFrom);
            var toType = typeof(TTo);
            var fromProps = fromType.GetProperties().Where(p => p.CanRead);
            var toProps = GetPrimitiveProps(toType).Where(p => p.CanWrite);

            foreach (var toProp in toProps)
            {
                foreach (var fromProp in fromProps)
                {
                    if (toProp.Name != fromProp.Name) continue; // || toProp.PropertyType != fromProp.PropertyType

                    toProp.SetValue(to, fromProp.GetValue(from)); break;
                }
            }

            return to;
        }

        /// <summary>
        /// Сравнить объекты различного типа
        /// </summary>
        /// <typeparam name="TA">Тип объекта А</typeparam>
        /// <typeparam name="TB">Тип объекта Б</typeparam>
        /// <param name="a">Объекта А</param>
        /// <param name="b">Объект Б</param>
        /// <param name="comparer">Функция сравнения</param>
        /// <returns>Логическое значение</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CompareDifferentObjects<TA, TB>(TA a, TB b, Func<TA, TB, bool> comparer)
        {
            return comparer(a, b);
        }

        /// <summary>
        /// Сравнить два значения
        /// </summary>
        /// <param name="valueA">Значение А</param>
        /// <param name="valueB">Значение Б</param>
        /// <returns>Логическое значение</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ValueComparer(object valueA, object valueB)
        {
            return valueA == valueB ||
                   (valueA != null && valueA.Equals(valueB)) ||
                   (valueB != null && valueB.Equals(valueA));
        }

        /// <summary>
        /// Функция сравнения объектов
        /// </summary>
        /// <typeparam name="T">Тип объектов</typeparam>
        /// <param name="oldItem">Предидущий объект</param>
        /// <param name="newItem">Обновлённый объект</param>
        /// <param name="props">Свойства объекта коллекции для сравнения</param>
        /// <param name="functor">Дополнительное сравнение</param>
        /// <returns>Логическое значение</returns>
        public static bool ValueComparer<T>(T oldItem, T newItem, PropertyInfo[] props, Func<T, T, bool> functor = null)
        {
            if (oldItem == null && newItem == null) return true;
            if (oldItem == null || newItem == null) return false;

            object oldValue, newValue;
            foreach (var prop in props)
            {
                oldValue = prop.GetValue(oldItem);
                newValue = prop.GetValue(newItem);
                if (ValueComparer(newValue, oldValue)) continue;
                return false;
            }
            return functor == null || functor(oldItem, newItem);
        }

        /// <summary>
        /// Функция сравнения коллекций
        /// </summary>
        /// <typeparam name="T">Тип коллекции</typeparam>
        /// <param name="oldItems">Предидущая колекция</param>
        /// <param name="newItems">Обновлённая колекция</param>
        /// <param name="props">Свойства объекта коллекции для сравнения</param>
        /// <param name="functor">Дополнительное сравнение</param>
        /// <returns>Логическое значение</returns>
        public static bool CollectionComparer<T>(IEnumerable<T> oldItems, IEnumerable<T> newItems, PropertyInfo[] props, Func<T, T, bool> functor = null)
        {
            var count = oldItems.Count();
            if (count != newItems.Count()) return false;
            if (count == 0) return true;
            if (props.Length == 0) return true;

            bool contain;
            int i;
            var found = new bool[count];
            object oldValue, newValue;

            foreach (var oldItem in oldItems)
            {
                contain = false;
                i = -1;
                foreach (var newItem in newItems)
                {
                    if (found[++i]) continue;
                    foreach (var prop in props)
                    {
                        oldValue = prop.GetValue(oldItem);
                        newValue = prop.GetValue(newItem);
                        if (ValueComparer(newValue, oldValue)) continue;
                        goto NextCompare;
                    }
                    found[i] = true;
                    contain = functor == null || functor(oldItem, newItem);
                    break;
                NextCompare: continue;
                }
                if (contain) continue;
                return false;
            }
            return true;
        }
    }
}