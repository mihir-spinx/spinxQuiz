using Spinx.Data.Infrastructure;
using Spinx.Domain.QuizAnswers;

namespace Spinx.Data.Repository.QuizAnswers
{
    public interface IQuizAnswerRepository : IRepository<QuizAnswer>
    {

    }

    public class QuizAnswerRepository : Repository<QuizAnswer>, IQuizAnswerRepository
    {
        public QuizAnswerRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
            
        }
    }
}