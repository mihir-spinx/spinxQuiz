using System.Collections.Generic;
using System.Linq;

namespace Spinx.Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        void Insert(T entity);
        void Insert(IEnumerable<T> entity);
        void Update(T entity);
        void Update(IList<T> entities);
        void Delete(T entity);
        void Delete(IList<T> entities);

        IEnumerable<T> All();
        T Find(int id);
        
        IQueryable<T> Table { get; }
        IQueryable<T> AsNoTracking { get; }

        string GenerateUniqueSlug(string phrse, int? id = null, string slugFieldName = "Slug");
    }
}