using Spinx.Services.Infrastructure;

namespace Spinx.Services.QuizAnswers.DTOs
{
    public class QuizAnswerAdminFilterDto : BaseFilterDto
    {
        public string Answer { get; set; }
        public int? QuizQuestionId { get; set; }
        public int? SortOrder { get; set; }
    }
}