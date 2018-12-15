namespace Spinx.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;
        private SqlContext _dataContext;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        protected SqlContext DataContext => _dataContext ?? (_dataContext = _databaseFactory.Get());

        public void Commit()
        {
            _databaseFactory.Get().Commit();
        }
    }
}