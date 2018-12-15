using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Spinx.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ToPaged<T>(this IQueryable<T> queryable, int pageIndex, int pageSize)
        {
            return queryable.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        private static PropertyInfo GetPropertyInfo(Type objType, string name)
        {
            var properties = objType.GetProperties();
            var matchedProperty = properties.FirstOrDefault(p => p.Name == name);
            if (matchedProperty == null)
                throw new ArgumentException("name");

            return matchedProperty;
        }
        private static LambdaExpression GetOrderExpression(Type objType, PropertyInfo pi)
        {
            var paramExpr = Expression.Parameter(objType);
            var propAccess = Expression.PropertyOrField(paramExpr, pi.Name);
            var expr = Expression.Lambda(propAccess, paramExpr);
            return expr;
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> query, string name)
        {
            var propInfo = GetPropertyInfo(typeof(T), name);
            var expr = GetOrderExpression(typeof(T), propInfo);

            var method = typeof(Enumerable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IEnumerable<T>)genericMethod.Invoke(null, new object[] { query, expr.Compile() });
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string name)
        {
            var propInfo = GetPropertyInfo(typeof(T), name);
            var expr = GetOrderExpression(typeof(T), propInfo);

            var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
        }

        public static IEnumerable<T> OrderByDescending<T>(this IEnumerable<T> query, string name)
        {
            var propInfo = GetPropertyInfo(typeof(T), name);
            var expr = GetOrderExpression(typeof(T), propInfo);

            var method = typeof(Enumerable).GetMethods().FirstOrDefault(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IEnumerable<T>)genericMethod.Invoke(null, new object[] { query, expr.Compile() });
        }

        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string name)
        {
            var propInfo = GetPropertyInfo(typeof(T), name);
            var expr = GetOrderExpression(typeof(T), propInfo);

            var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
        }

        //public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string[] withinColumns, string defaultOrderBy = "CreatedAt")
        //{
        //    var orderBy = withinColumns.Contains(System.Web.HttpContext.Current.Request["sortColumn"])
        //        ? System.Web.HttpContext.Current.Request["sortColumn"]
        //        : defaultOrderBy;

        //    var sortType = System.Web.HttpContext.Current.Request["sortType"] ?? "desc";

        //    return sortType.ToLower() == "desc"
        //        ? query.OrderByDescending(orderBy)
        //        : query.OrderBy(orderBy);
        //}

        /// <summary>
        /// Excepts the specified other.
        /// Compare values from two lists and return entities from one list which is not present into second list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="other">The other.</param>
        /// <param name="getKey">The get key.</param>
        /// <returns>IEnumerable.</returns>
        public static IEnumerable<T> Except<T, TKey>(this IEnumerable<T> items, IEnumerable<T> other, 
            Func<T, TKey> getKey)
        {
            return from item in items
                join otherItem in other on getKey(item)
                    equals getKey(otherItem) into tempItems
                from temp in tempItems.DefaultIfEmpty()
                where ReferenceEquals(null, temp) || temp.Equals(default(T))
                select item;
        }
    }
}