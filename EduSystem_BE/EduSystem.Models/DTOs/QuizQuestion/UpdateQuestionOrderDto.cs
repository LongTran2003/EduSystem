using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTOs.QuizQuestion
{
    public class UpdateQuestionOrderDto
    {
        [Required(ErrorMessage = "QuizId is required")]
        public Guid QuizId { get; set; }

        [Required(ErrorMessage = "QuestionId is required")]
        public Guid QuestionId { get; set; }

        [Required(ErrorMessage = "NewOrder is required")]
        [Range(1, int.MaxValue, ErrorMessage = "NewOrder must be greater than 0")]
        public int NewOrder { get; set; }
    }
}
