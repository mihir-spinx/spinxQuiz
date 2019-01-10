using Spinx.Data.Infrastructure;
using System.Linq;

namespace Spinx.Data.Repository.Member
{
    public interface IMemberQuizAnswerOptionsRepository : IRepository<Domain.Members.MemberQuizAnswerOptions>
    {
        //Member FindActiveUserByEmail(string email);
    }

    public class MemberQuizAnswerOptionsRepository : Repository<Domain.Members.MemberQuizAnswerOptions>, IMemberQuizAnswerOptionsRepository
    {
        public MemberQuizAnswerOptionsRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }       
    }
}