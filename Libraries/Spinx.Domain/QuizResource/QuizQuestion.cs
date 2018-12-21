using Spinx.Core.Domain;
using Spinx.Domain.QuizAnswers;
using Spinx.Domain.Quizs;
using System;
using System.Collections.Generic;

namespace Spinx.Domain.QuizQuestions
{
    public class QuizQuestion : IModificationHistory
    {
        public int Id { get; set; }
        public string Question { get; set; }

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public int? SortOrder { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<QuizAnswer> QuizAnswer { get; set; }
    }
}