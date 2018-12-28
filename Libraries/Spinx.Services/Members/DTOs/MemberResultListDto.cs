using Spinx.Domain.Members;
using Spinx.Domain.Quizs;
using Spinx.Services.Infrastructure;
using System;

namespace Spinx.Services.Members.DTOs
{
    public class MemberResultListDto : BaseFilterDto
    {
        public int Id { get; set; }
        public int? AttempedQues { get; set; }

        public int? FromAttempedQues { get; set; }
        public int? ToAttempedQues { get; set; }

        public int? FromScore { get; set; }
        public int? ToScore { get; set; }

        public int? FromPercentage { get; set; }
        public int? ToPercentage { get; set; }

        public int? Score { get; set; }

        public decimal? Percentage { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string College { get; set; }
        public int CreatedSource { get; set; }

        public Quiz Quiz { get; set; }
        public string QuizTitle { get; set; }
        public string QuizCategoryName { get; set; }
        public int QuizQuestions { get; set; }



    }
}
