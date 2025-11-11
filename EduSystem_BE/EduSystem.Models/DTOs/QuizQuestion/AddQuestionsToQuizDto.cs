using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTOs.QuizQuestion
{
    public class AddQuestionsToQuizDto
    {
        [Required(ErrorMessage = "Quiz ID is required")]
        public Guid QuizId { get; set; }

        [Required(ErrorMessage = "Question IDs are required")]
        [MinLength(1, ErrorMessage = "At least one question ID is required")]
        public List<Guid> QuestionIds { get; set; } = new List<Guid>();

    }
}
