using Spinx.Data.Infrastructure;
using System.Linq;

namespace Spinx.Data.Repository.Member
{
    public interface IMemberQuizAnswerRepository : IRepository<Domain.Members.MemberQuizAnswer>
    {
        //Member FindActiveUserByEmail(string email);
    }

    public class MemberQuizAnswerRepository : Repository<Domain.Members.MemberQuizAnswer>, IMemberQuizAnswerRepository
    {
        public MemberQuizAnswerRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }       
    }
}