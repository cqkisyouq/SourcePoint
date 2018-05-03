using EFSecondLevelCache.Core;
using EFSecondLevelCache.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;
using SourcePoint.Infrastructure.Extensions.ODataExtension.ExpressionVisitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SourcePoint.Infrastructure.Extensions.ODataExtension.Cache
{
    public static class SecondCacheQuerableExtensions
    {
        private static MethodInfo _CacheQuerableMethod = typeof(SecondCacheQuerableExtensions).GetMethod(nameof(ODataCacheHandle));
        private static MethodInfo _CacheValue = typeof(SecondCacheQuerableExtensions).GetMethod(nameof(ODataGetCacheValue));
        private static MethodInfo _CreateListType = typeof(SecondCacheQuerableExtensions).GetMethod(nameof(CreateListType));

        private static Dictionary<Type, MethodInfo> _MethodInfoPool = new Dictionary<Type, MethodInfo>();
        public static void ODataCacheQuerable(this IQueryable queryable, object value, IServiceProvider serviceProvider)
        {
            var cache = _CacheQuerableMethod.MakeGenericMethod(queryable.ElementType);
            queryable = cache.Invoke(null, new object[] { queryable, value, serviceProvider }) as IQueryable;
        }
       /// <summary>
       /// 从缓存中获取数据
       /// </summary>
       /// <param name="queryable"></param>
       /// <param name="cacheServiceProvider"></param>
       /// <returns></returns>
        public static object ODataCacheResult(this IQueryable queryable, IServiceProvider cacheServiceProvider)
        {
            var cache = _CacheValue.MakeGenericMethod(queryable.ElementType);
            var result = cache.Invoke(null, new object[] { queryable, cacheServiceProvider });
            return result;
        }

        public static object ODataGetCacheValue<T>(IQueryable<T> queryable, IServiceProvider serviceProvider)
        {
            var cacheProvider = serviceProvider.GetService<IEFCacheKeyProvider>();
            var cacheService = serviceProvider.GetService<IEFCacheServiceProvider>();
            var cacheKey = CacheKey(queryable, cacheProvider);
            var queryCacheKey = cacheKey.KeyHash;
            var result = cacheService.GetValue(queryCacheKey);
            return result;
        }

        public static void ODataCacheHandle<T>(IQueryable<T> query, object value, IServiceProvider serviceProvider)
        {
            var cacheKeyProvider = serviceProvider.GetService<IEFCacheKeyProvider>();
            var cacheServiceProvider = serviceProvider.GetService<IEFCacheServiceProvider>();
            var cacheKey = CacheKey(query, cacheKeyProvider);
            cacheServiceProvider.InsertValue(cacheKey.KeyHash, value, cacheKey.CacheDependencies);
        }

        private static Type MarkListType(this Type type)
        {
            type = type.IsGenericType ? type.GenericTypeArguments[0] : type;

            if (type.DeclaringType != null && type.DeclaringType.Name.Equals("SelectExpandBinder", StringComparison.OrdinalIgnoreCase))
                type = type.GenericTypeArguments[0];

            type = _CreateListType.MakeGenericMethod(type).Invoke(null, null) as Type;
            return type;
        }
        private static Type CreateListType<T>()
        {
            return typeof(List<T>);
        }

        private static string SaltKey(Expression expression)
        {
            ConstantVisitor constantVisitor = new ConstantVisitor();
            constantVisitor.Visit(expression);
            var saltKey = constantVisitor.ValueBuilder.ToString();
            saltKey = $"{XxHashUnsafe.ComputeHash(saltKey):X}";
            return saltKey;
        }

        private static EFCacheKey CacheKey<T>(IQueryable<T> queryable, IEFCacheKeyProvider cacheKeyProvider)
        {
            var saltKey = SaltKey(queryable.Expression);
            var cacheKey = cacheKeyProvider.GetEFCacheKey(queryable, queryable.Expression, saltKey);
            return cacheKey;
        }
    }
}
