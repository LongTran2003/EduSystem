using EduSystem.Models.DTOs.StudentAnswers;
using EduSystem.Utilities.Contants;
using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTOs.QuizAttempt
{
    public class CreateQuizAttemptDto
    {
        public Guid QuizId { get; set; }
        public Guid StudentId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Score { get; set; }
        public List<CreateStudentAnswerDto>? StudentAnswers { get; set; } = new List<CreateStudentAnswerDto>();
    }
}
