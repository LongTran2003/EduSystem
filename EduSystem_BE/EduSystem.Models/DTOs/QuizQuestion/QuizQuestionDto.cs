namespace EduSystem.Models.DTOs.QuizQuestion
{
    public class QuizQuestionDto
    {
        public Guid QuizId { get; set; }
        public string? QuizTitle { get; set; }
        public Guid QuestionId { get; set; }
        public string? QuestionText { get; set; }
        public string? QuestionType { get; set; }
        public int QuestionOrder { get; set; }

        public string Status { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
