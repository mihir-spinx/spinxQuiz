using Spinx.Data.Infrastructure;
using System.Linq;

namespace Spinx.Data.Repository.Member
{
    public interface IMemberRepository : IRepository<Domain.Members.Member>
    {
        //Member FindActiveUserByEmail(string email);
    }

    public class MemberRepository : Repository<Domain.Members.Member>, IMemberRepository
    {
        public MemberRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public Domain.Members.Member FindActiveUserByEmail(string email)
        {
            return AsNoTracking.FirstOrDefault(t => t.Email == email && t.IsActive);
        }
    }
}