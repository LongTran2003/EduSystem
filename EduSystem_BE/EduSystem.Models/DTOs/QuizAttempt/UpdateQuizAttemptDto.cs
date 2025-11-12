using EduSystem.Models.DTOs.StudentAnswers;

namespace EduSystem.Models.DTOs.QuizAttempt
{
    public class UpdateQuizAttemptDto
    {
        public Guid QuizAttemptId { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal? Score { get; set; }
        public string? Feedback { get; set; }
    }
}
