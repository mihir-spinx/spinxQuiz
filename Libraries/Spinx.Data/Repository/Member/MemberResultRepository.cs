using Spinx.Data.Infrastructure;
using System.Linq;

namespace Spinx.Data.Repository.Member
{
    public interface IMemberResultRepository : IRepository<Domain.Members.MemberResult>
    {
        //Member FindActiveUserByEmail(string email);
    }

    public class MemberResultRepository : Repository<Domain.Members.MemberResult>, IMemberResultRepository
    {
        public MemberResultRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }       
    }
}