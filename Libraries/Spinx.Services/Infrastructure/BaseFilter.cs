using System;
using System.Linq;
using System.Reflection;

namespace Spinx.Services.Infrastructure
{
    public abstract class BaseFilter<TEntity, TDto> 
        where TEntity : class
        where TDto : class 
    {
        protected IQueryable<TEntity> Query;
        protected readonly TDto Dto;

        protected BaseFilter(IQueryable<TEntity> query, TDto dto)
        {
            Query = query;
            Dto = dto;
        }

        public IQueryable<TEntity> FilteredQuery()
        {
            if (Dto == null) return Query;

            var filterMethodInfos = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
            var dtoProperties = typeof(TDto).GetProperties();

            // from base class properties
            var ignoreProperties = new[] { "Action", "Ids", "Page", "Size", "SortColumn", "SortType" };

            foreach (var propertyInfo in dtoProperties.Where(w => !ignoreProperties.Contains(w.Name)))
            {
                var typePassed = false;

                if (propertyInfo.PropertyType == typeof(string))
                    typePassed = StringMethodCall(propertyInfo);
                else if (propertyInfo.PropertyType == typeof(int?) || 
                         propertyInfo.PropertyType == typeof(bool?) ||
                         propertyInfo.PropertyType == typeof(decimal?) ||
                         propertyInfo.PropertyType == typeof(DateTime?))
                    typePassed = NullableMethodCall(propertyInfo);

                if (!typePassed) continue;

                var method = filterMethodInfos.FirstOrDefault(w => w.Name == propertyInfo.Name);

                if (method != null)
                    method.Invoke(this, null);
            }

            return Query;
        }

        private bool StringMethodCall(PropertyInfo propertyInfo)
        {
            var value = Convert.ToString(propertyInfo.GetValue(Dto, null));

            return !string.IsNullOrEmpty(value);
        }

        private bool NullableMethodCall(PropertyInfo propertyInfo)
        {
            var value = propertyInfo.GetValue(Dto, null);

            return value != null;
        }
    }
}