using Spinx.Data.Infrastructure;
using Spinx.Domain.QuizQuestions;

namespace Spinx.Data.Repository.QuizQuestions
{
    public interface IQuizQuestionRepository : IRepository<QuizQuestion>
    {

    }

    public class QuizQuestionRepository : Repository<QuizQuestion>, IQuizQuestionRepository
    {
        public QuizQuestionRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
            
        }
    }
}