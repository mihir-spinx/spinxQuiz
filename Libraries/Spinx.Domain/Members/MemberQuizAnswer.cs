using Spinx.Core.Domain;
using Spinx.Domain.QuizAnswers;
using Spinx.Domain.QuizQuestions;
using Spinx.Domain.Quizs;
using System;

namespace Spinx.Domain.Members
{
    public class MemberQuizAnswer : IModificationHistory
    {
        public int Id { get; set; }

        public int MemberResultId { get; set; }
        public MemberResult MemberResult { get; set; }                

        public int? QuizAnswerId { get; set; }
        public QuizAnswer QuizAnswer { get; set; }

        public int QuizQuestionId { get; set; }
        public QuizQuestion QuizQestion { get; set; }

        public bool IsRight { get; set; }
        public bool? IsAttempt  { get; set; }

        public int? SortOrder { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
