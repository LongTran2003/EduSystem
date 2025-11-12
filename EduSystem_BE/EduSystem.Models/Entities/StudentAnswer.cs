using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class StudentAnswer : BaseEntity<string, string, string>
    {
        [Key]
        public Guid StudentAnswerId { get; set; }

        public Guid AttemptId { get; set; }
        [ForeignKey("AttemptId")]
        public virtual QuizAttempt QuizAttempt { get; set; } = null!;

        public Guid QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; } = null!;

        public Guid? AnswerId { get; set; }
        [ForeignKey("AnswerId")]
        public virtual Answer? Answer { get; set; }

        public string? Answers { get; set; }

        public bool? IsCorrect { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Score { get; set; }

        public string? TeacherFeedback { get; set; }
    }
}
