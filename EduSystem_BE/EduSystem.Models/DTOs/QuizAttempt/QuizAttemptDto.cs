using EduSystem.Models.DTOs.StudentAnswers;

namespace EduSystem.Models.DTOs.QuizAttempt
{
    public class QuizAttemptDto
    {
        public Guid QuizAttemptId { get; set; }
        public Guid StudentId { get; set; }
        public string? StudentName { get; set; }
        public Guid QuizId { get; set; }
        public string? QuizName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal? Score { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Feedback { get; set; }
        public List<StudentAnswerDto>? StudentAnswers { get; set; } = new List<StudentAnswerDto>();
        public string? CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
