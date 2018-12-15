using Spinx.Data.Infrastructure;
using Spinx.Domain.QuizCategories;

namespace Spinx.Data.Repository.Quizs
{
    public interface IQuizCategoryRepository : IRepository<QuizCategory>
    {
    }

    public class QuizCategoryRepository : Repository<QuizCategory>, IQuizCategoryRepository
    {
        public QuizCategoryRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}