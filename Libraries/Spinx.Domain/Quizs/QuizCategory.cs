using Spinx.Core.Domain;
using Spinx.Domain.Quizs;
using System;
using System.Collections.Generic;

namespace Spinx.Domain.QuizCategories
{
    public class QuizCategory : IModificationHistory
    {
        public int Id { get; set; }      

        public string Name { get; set; }
        public string Slug { get; set; }
        public string CategoryIcon { get; set; }
        public int? SortOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Quiz> Quizs { get; set;}
    }
}