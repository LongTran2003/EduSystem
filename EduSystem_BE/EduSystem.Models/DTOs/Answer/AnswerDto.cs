namespace EduSystem.Models.DTOs.Answer
{
    public class AnswerDto
    {
        public Guid AnswerId { get; set; }
        public Guid QuestionId { get; set; }
        public string Content { get; set; } = null!;
        public bool IsCorrect { get; set; }
        public string? Explanation { get; set; }
        public string Status { get; set; } = null!;
        public string? CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
