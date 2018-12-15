using Omu.ValueInjecter;
using Spinx.Domain.Quizs;
using Spinx.Services.Quizs.DTOs;

namespace Spinx.Services.Quizs.Mappers
{
    public static class QuizAdminMapper
    {
        public static void Init()
        {
            Mapper.AddMap<QuizCreateAdminDto, Quiz>((from, to) =>
            {
                var existing = to as Quiz ?? new Quiz();
                existing.InjectFrom(from);
                return existing;
            });

            Mapper.AddMap<QuizEditAdminDto, Quiz>((from, to) =>
            {
                var existing = to as Quiz ?? new Quiz();
                existing.InjectFrom(from);
                return existing;
            });
        }
    }
}
