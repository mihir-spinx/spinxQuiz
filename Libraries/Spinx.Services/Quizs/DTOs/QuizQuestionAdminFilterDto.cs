using Spinx.Services.Infrastructure;

namespace Spinx.Services.QuizQuestions.DTOs
{
    public class QuizQuestionAdminFilterDto : BaseFilterDto
    {
        public string Question { get; set; }
        public bool? IsActive { get; set; }
        public int? QuizId { get; set; }
        public int? SortOrder { get; set; }
    }
}