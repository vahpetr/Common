using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Common.Utilites;

namespace Common.Extensions
{
    public static class MappingExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IQueryable<TTo> Map<TFrom, TTo>(this IQueryable<TFrom> source) where TFrom : class where TTo : class
        {
            return source.Select(MappingExpressionUtilites<TFrom, TTo>.Mapper);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TTo> Map<TFrom, TTo>(this IEnumerable<TFrom> source) where TFrom : class where TTo : class
        {
            return source.Select(MappingUtilites<TFrom, TTo>.Mapper);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TTo Map<TFrom, TTo>(this TFrom source) where TFrom : class where TTo : class
        {
            return MappingUtilites<TFrom, TTo>.Mapper(source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TTo Map<TTo>(this Dictionary<string, object> mapping) where TTo : class
        {
            return MappingUtilites<TTo>.Mapper(mapping);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TTo Rolled<TFrom, TTo>(this TFrom from, TTo to)
        {
            return ObjectUtilites.Rolled(from, to);
        }
    }
}