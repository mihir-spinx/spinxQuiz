using Spinx.Services.Infrastructure;

namespace Spinx.Services.Quizs.DTOs
{
    public class QuizAdminFilterDto : BaseFilterDto
    {
        public string Title { get; set; }
        public int? QuizCategoryId { get; set; }
        public bool? IsActive { get; set; }
    }
}