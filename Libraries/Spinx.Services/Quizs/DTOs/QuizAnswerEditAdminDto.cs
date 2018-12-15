namespace Spinx.Services.QuizAnswers.DTOs
{
    public class QuizAnswerEditAdminDto
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public bool IsCorrectAnswer { get; set; }
        public int SortOrder { get; set; }
    }
}
