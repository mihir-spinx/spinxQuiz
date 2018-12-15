using Spinx.Services.Infrastructure;

namespace Spinx.Services.QuizCategories.DTOs
{
    public class QuizCategoryAdminFilterDto : BaseFilterDto
    {
        public string Name { get; set; }
        public bool? IsActive { get; set; }
    }
}