using Spinx.Core.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Spinx.Services.Infrastructure
{
    public class BaseListOrder<TEntity> where TEntity : class 
    {
        protected IQueryable<TEntity> Query;
        protected string SortColumn;
        protected SortType SortType;

        private const string DefaultSortColumn = "CreatedAt";
        private const SortType DefaultSortType = SortType.Desc;
        

        protected BaseListOrder(IQueryable<TEntity> query, BaseFilterDto dto)
        {
            Query = query;
            
            SortColumn = SetSortColumn(dto?.SortColumn ?? DefaultSortColumn);
            SortType = SetSortType(dto?.SortType ?? DefaultSortType.ToString());
        }

        private string SetSortColumn(string sortColumn)
        {
            var methodInfos = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);

            var methodName = methodInfos.FirstOrDefault(w => w.Name.ToLower() == sortColumn.NullSafeToLower())?.Name;

            return methodName ?? DefaultSortColumn;
        }

        private static SortType SetSortType(string sortType)
        {
            switch (sortType)
            {
                case "asc":
                    return SortType.Asc;
                case "desc":
                    return SortType.Desc;
            }

            return DefaultSortType;
        }

        internal IQueryable<TEntity> OrderBy<TKey>(Expression<Func<TEntity, TKey>> keySelector)
        {
            return SortType == SortType.Asc ? 
                Query.OrderBy(keySelector) :
                Query.OrderByDescending(keySelector);
        }

        public IQueryable<TEntity> OrderByQuery()
        {
            var methodInfos = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);

            var methodInfo = methodInfos.FirstOrDefault(w => w.Name == SortColumn);

            if (methodInfo == null)
                throw new Exception($"Invalid order by column {SortColumn} in {typeof(TEntity).Name}.");

            methodInfo.Invoke(this, null);

            return Query;
        }
    }
}