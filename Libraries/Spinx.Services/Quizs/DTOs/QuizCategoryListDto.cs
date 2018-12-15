using Spinx.Domain.Quizs;
using System.Collections.Generic;

namespace Spinx.Services.QuizCategories.DTOs
{
    public class QuizCategoryListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string CategoryIcon { get; set; }
        public List<Quiz> Quiz { get; set; }
        public int? SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}