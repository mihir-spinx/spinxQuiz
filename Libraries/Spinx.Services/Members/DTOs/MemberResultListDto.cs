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
        public int? Score { get; set; }
        public decimal? Percentage { get; set; }
        public DateTime? TestStartTime { get; set; }
        public DateTime? TestEndTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public TimeSpan? TimeDuration { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int CreatedSource { get; set; }
        public string CreatedSourceName { get; set; }
        public int? QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public int QuizQuestions { get; set; }

    }
}
