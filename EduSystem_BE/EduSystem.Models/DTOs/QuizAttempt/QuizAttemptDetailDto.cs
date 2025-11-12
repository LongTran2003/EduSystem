using EduSystem.Models.DTOs.StudentAnswers;

namespace EduSystem.Models.DTOs.QuizAttempt
{
    public class QuizAttemptDetailDto : QuizAttemptDto
    {
        public List<StudentAnswerDto> StudentAnswers { get; set; } = new();
    }
}
