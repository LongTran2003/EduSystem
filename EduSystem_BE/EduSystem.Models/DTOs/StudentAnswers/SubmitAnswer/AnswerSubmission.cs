using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTOs.StudentAnswers.SubmitAnswer
{
    public class AnswerSubmission
    {
        [Required]
        public Guid QuestionId { get; set; }

        public Guid? AnswerId { get; set; }

        [StringLength(1000)]
        public string? Answers { get; set; }
    }
}
