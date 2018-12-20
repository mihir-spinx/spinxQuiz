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

        public int MemberId { get; set; }
        public Member Member { get; set; }

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public int QuizAnswerId { get; set; }
        public QuizAnswer QuizAnswer { get; set; }

        public int QuizQuestionId { get; set; }
        public QuizQuestion QuizQestion { get; set; }

        public bool IsRight { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
