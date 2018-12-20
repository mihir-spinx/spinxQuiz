using Spinx.Domain.QuizAnswers;
using Spinx.Services.Infrastructure;
using System.Collections.Generic;

namespace Spinx.Services.QuizQuestionsList.DTOs
{
    public class QuizQuestionCountDto 
    {
        public int Id { get; set; }
        public int? SortOrder { get; set; }        
    }
}