using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTOs.QuizQuestion
{
    public class CreateQuizQuestionDto
    {
        [Required(ErrorMessage = "Quiz ID is required")]        
        public Guid QuizId { get; set; }

        [Required(ErrorMessage = "Question ID is required")]
        public Guid QuestionId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Question order must be greater than 0")]
        public int QuestionOrder { get; set; } = 0;
    }
}
