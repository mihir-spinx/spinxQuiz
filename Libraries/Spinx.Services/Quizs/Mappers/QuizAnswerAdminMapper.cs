using Omu.ValueInjecter;
using Spinx.Domain.QuizAnswers;
using Spinx.Services.QuizAnswers.DTOs;

namespace Spinx.Services.QuizAnswers.Mappers
{
    public static class QuizAnswerAdminMapper
    {
        public static void Init()
        {
            Mapper.AddMap<QuizAnswerCreateAdminDto, QuizAnswer>((from, to) =>
            {
                var existing = to as QuizAnswer ?? new QuizAnswer();
                existing.InjectFrom(from);
                return existing;
            });

            Mapper.AddMap<QuizAnswerEditAdminDto, QuizAnswer>((from, to) =>
            {
                var existing = to as QuizAnswer ?? new QuizAnswer();
                existing.InjectFrom(from);
                return existing;
            });
        }
    }
}
