using Omu.ValueInjecter;
using Spinx.Domain.QuizCategories;
using Spinx.Services.QuizCategories.DTOs;

namespace Spinx.Services.QuizCategories.Mappers
{
    public static class QuizCategoryAdminMapper
    {
        public static void Init()
        {
            Mapper.AddMap<QuizCategoryCreateAdminDto, QuizCategory>((from, to) =>
            {
                var existing = to as QuizCategory ?? new QuizCategory();
                existing.InjectFrom(from);
                return existing;
            });

            Mapper.AddMap<QuizCategoryEditAdminDto, QuizCategory>((from, to) =>
            {
                var existing = to as QuizCategory ?? new QuizCategory();
                existing.InjectFrom(from);
                return existing;
            });
        }
    }
}
