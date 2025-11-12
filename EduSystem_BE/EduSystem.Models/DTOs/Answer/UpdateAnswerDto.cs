using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTOs.Answer
{
    public class UpdateAnswerDto
    {
        [Required]
        public Guid AnswerId { get; set; }

        public Guid? QuestionId { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        [Required]
        public bool IsCorrect { get; set; }

        public string? Explanation { get; set; }
    }
}
