using System;
using Spinx.Domain.Members;
using Spinx.Domain.Quizs;
using System.Collections.Generic;

namespace Spinx.Services.QuizCategories.DTOs
{
    public class MemberQuizListDto
    {
        public DateTime StartTime { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public int MemberResultId { get; set; }
        public int? SortOrder { get; set; }
        public List<MemberQuizAnswer> MemberQuizAnswerList { get; set; }        
    }
}