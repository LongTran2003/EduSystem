using EduSystem.Models.DTOs.StudentAnswers;

namespace EduSystem.Models.DTOs.QuizAttempt
{
    public class UpdateQuizAttemptDto
    {
        public Guid QuizAttemptId { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal? Score { get; set; }
        public List<UpdateStudentAnswerDto>? StudentAnswers { get; set; } = new List<UpdateStudentAnswerDto>();
    }
}
