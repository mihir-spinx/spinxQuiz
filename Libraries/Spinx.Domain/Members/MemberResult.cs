using Spinx.Core.Domain;
using Spinx.Domain.Quizs;
using System;
using System.Collections.Generic;

namespace Spinx.Domain.Members
{
    public class MemberResult : IModificationHistory
    {
        public int Id { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; }

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public int AttempedQues { get; set; }
        public int Score { get; set; }
        public decimal Percentage { get; set; }        

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<MemberQuizAnswer> MemberQuizAnswer { get; set; }
    }
}