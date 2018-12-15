namespace Spinx.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}