using Omu.ValueInjecter;
using Spinx.Domain.QuizQuestions;
using Spinx.Services.QuizQuestions.DTOs;

namespace Spinx.Services.QuizQuestions.Mappers
{
    public static class QuizQuestionAdminMapper
    {
        public static void Init()
        {
            Mapper.AddMap<QuizQuestionCreateAdminDto, QuizQuestion>((from, to) =>
            {
                var existing = to as QuizQuestion ?? new QuizQuestion();
                existing.InjectFrom(from);
                return existing;
            });

            Mapper.AddMap<QuizQuestionEditAdminDto, QuizQuestion>((from, to) =>
            {
                var existing = to as QuizQuestion ?? new QuizQuestion();
                existing.InjectFrom(from);
                return existing;
            });
        }
    }
}
