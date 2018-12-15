using Spinx.Core.Extensions;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;

namespace Spinx.Data.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private SqlContext _dataContext;
        private readonly IDbSet<T> _dbSet;

        protected IDatabaseFactory DatabaseFactory { get; }
        protected SqlContext DataContext => _dataContext ?? (_dataContext = DatabaseFactory.Get());

        public Repository(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;           
           _dbSet = databaseFactory.Get().Set<T>();
        }

        public void Insert(T entity)
        {
            _dbSet.Add(entity);
            DataContext.Entry(entity).State = EntityState.Added;
        }

        public void Insert(IEnumerable<T> entities)
        {
            DataContext.Set<T>().AddRange(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            DataContext.Entry(entity).State = EntityState.Modified;
        }

        public void Update(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                DataContext.Set<T>().Attach(entity);
                var entry = DataContext.Entry(entity);
                entry.State = EntityState.Modified;
            }
        }

        public void Delete(T entity)
        {
            _dbSet.Attach(entity);
            _dbSet.Remove(entity);
            DataContext.Entry(entity).State = EntityState.Deleted;
        }

        public void Delete(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                _dbSet.Attach(entity);
                _dbSet.Remove(entity);
                DataContext.Entry(entity).State = EntityState.Deleted;
            }
        }

        public IEnumerable<T> All()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public T Find(int id)
        {
            return _dbSet.Find(id);
        }

        public string GenerateUniqueSlug(string phrse, int? id = null, string slugFieldName = "Slug")
        {
            int? loop = null;
            var slug = phrse.GenerateSlug();

            var where = $"{slugFieldName} = @0";
            if (id != null)
                where += " AND Id <> @1";

            while (AsNoTracking.Where(where,slug, id).Count() > 0)
            {
                loop = loop == null ? 1 : loop + 1;
                slug = phrse.GenerateSlug() + ("-" + loop);
            }

            return slug;
        }

        public IQueryable<T> Table => _dbSet;
        public IQueryable<T> AsNoTracking => _dbSet.AsNoTracking();
    }
}