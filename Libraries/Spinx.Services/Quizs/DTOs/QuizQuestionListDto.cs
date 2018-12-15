using Spinx.Domain.QuizAnswers;
using Spinx.Services.Infrastructure;
using System.Collections.Generic;

namespace Spinx.Services.QuizQuestionsList.DTOs
{
    public class QuizQuestionListDto : BaseFilterDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<QuizAnswer> QuizAnswers { get; set; }
    }
}