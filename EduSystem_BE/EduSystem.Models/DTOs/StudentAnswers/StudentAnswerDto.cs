namespace EduSystem.Models.DTOs.StudentAnswers
{
    public class StudentAnswerDto
    {
        public Guid AttemptId { get; set; }
        public Guid QuestionId { get; set; }
        public string? QuestionText { get; set; }
        public Guid? AnswerId { get; set; }
        public string? AnswerText { get; set; }
        public string? Answers { get; set; }
        public bool? IsCorrect { get; set; }
        public decimal? Score { get; set; }
        public string? TeacherFeedback { get; set; }
        public string Status { get; set; } = null!;
        public string? CreatedBy { get; set; } = null!;
        public DateTime CreatedTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
