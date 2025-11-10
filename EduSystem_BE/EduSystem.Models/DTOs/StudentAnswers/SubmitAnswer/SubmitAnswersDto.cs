using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTOs.StudentAnswers.SubmitAnswer
{
    public class SubmitAnswersDto
    {
        public Guid AttemptId { get; set; }

        [MinLength(1)]
        public List<AnswerSubmission> Answers { get; set; } = new();
    }
}
