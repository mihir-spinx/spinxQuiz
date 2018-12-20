using Spinx.Core.Domain;
using Spinx.Domain.QuizCategories;
using Spinx.Domain.QuizQuestions;
using System;
using System.Collections.Generic;

namespace Spinx.Domain.Quizs
{
    public class Quiz : IModificationHistory
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Slug { get; set; }
        public string ShortDescription { get; set; }

        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }

        public int QuizCategoryId { get; set; }
        public QuizCategory QuizCategory { get; set; }
        public List<QuizQuestion> QuizQuestions { get; set; }

        public int? SortOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}