namespace Spinx.Services.QuizAnswers.DTOs
{
    public class QuizAnswerCreateAdminDto
    {
        public string Answer { get; set; }
        public bool IsCorrectAnswer { get; set; }
        public int QuizQuestionId { get; set; }
    }
}
