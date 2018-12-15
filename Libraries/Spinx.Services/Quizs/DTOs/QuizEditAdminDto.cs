namespace Spinx.Services.Quizs.DTOs
{
    public class QuizEditAdminDto
    {
        public int Id { get; set; }      
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Slug { get; set; }
        public int QuizCategoryId { get; set; }
        public bool IsActive { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
    }
}
