namespace Spinx.Services.Quizs.DTOs
{
    public class QuizCreateAdminDto
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string ShortDescription { get; set; }
        public int QuizCategoryId { get; set; }
        public bool IsActive { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
    }
}
