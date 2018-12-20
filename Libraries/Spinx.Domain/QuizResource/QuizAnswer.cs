using Spinx.Domain.QuizQuestions;

namespace Spinx.Domain.QuizAnswers
{
    public class QuizAnswer 
    {
        public int Id { get; set; }      
        public string Answer { get; set; }
        public bool IsCorrectAnswer { get; set; }

        public int QuizQuestionId { get; set; }
        public QuizQuestion QuizQuestion { get; set; }

        public int? SortOrder { get; set; }
    }
}