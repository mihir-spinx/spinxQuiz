using Spinx.Domain.QuizAnswers;
using Spinx.Services.Infrastructure;

namespace Spinx.Services.QuizAnswersList.DTOs
{
    public class QuizAnswerListDto : BaseFilterDto
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public bool IsCorrectAnswer { get; set; }
    }
}