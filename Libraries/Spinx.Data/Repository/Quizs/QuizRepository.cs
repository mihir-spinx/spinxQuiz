using Spinx.Data.Infrastructure;
using Spinx.Domain.Quizs;

namespace Spinx.Data.Repository.Quizs
{
    public interface IQuizRepository : IRepository<Quiz>
    {

    }

    public class QuizRepository : Repository<Quiz>, IQuizRepository
    {
        public QuizRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
            
        }
    }
}