using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTOs.StudentAnswers
{
    public class UpdateStudentAnswerDto
    {
        public Guid AttemptId { get; set; }

        public Guid QuestionId { get; set; }

        public Guid? AnswerId { get; set; }

        [StringLength(1000)]
        public string? Answers { get; set; }

        public bool? IsCorrect { get; set; }

        [Range(0, 100)]
        public decimal? Score { get; set; }

        [StringLength(500)]
        public string? TeacherFeedback { get; set; }
    }
}
