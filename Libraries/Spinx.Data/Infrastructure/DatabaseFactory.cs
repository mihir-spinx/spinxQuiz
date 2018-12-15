namespace Spinx.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private SqlContext _dataContext;

        public SqlContext Get()
        {
            return _dataContext ?? (_dataContext = new SqlContext());
        }

        protected override void DisposeCore()
        {
            _dataContext?.Dispose();
            base.DisposeCore();
        }
    }
}