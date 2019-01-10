using Spinx.Core.Domain;
using Spinx.Domain.QuizAnswers;
using Spinx.Domain.QuizQuestions;
using Spinx.Domain.Quizs;
using System;

namespace Spinx.Domain.Members
{
    public class MemberQuizAnswerOptions 
    {
        public int Id { get; set; }

        public int MemberQuizAnswerId { get; set; }
        public MemberQuizAnswer MemberQuizAnswer { get; set; }

        public int? QuizAnswerId { get; set; }
        public QuizAnswer QuizAnswer { get; set; }

        public int? SortOrder { get; set; }
    }
}
