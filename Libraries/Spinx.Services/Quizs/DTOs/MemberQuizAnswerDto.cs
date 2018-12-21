using Spinx.Domain.Members;
using Spinx.Domain.Quizs;
using System.Collections.Generic;

namespace Spinx.Services.QuizCategories.DTOs
{
    public class MemberQuizAnswerDto
    {        
        public int MemberResultId { get; set; }
        public int? QuizAnswerId { get; set; }
        public int QuizQuestionId { get; set; }
    }
}