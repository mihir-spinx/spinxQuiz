namespace Spinx.Services.QuizQuestions.DTOs
{
    public class QuizQuestionEditAdminDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public int QuizId { get; set; }
    }
}
