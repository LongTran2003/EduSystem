using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class Answer : BaseEntity<string, string, string>
    {
        [Key]
        public Guid AnswerId { get; set; }

        [StringLength(500)]
        public string AnswerText { get; set; } = null!; // Nội dung đáp án

        public bool IsCorrect { get; set; } = false; // Đáp án đúng hay sai

        [StringLength(200)]
        public string? Explanation { get; set; } // Giải thích đáp án

        // Foreign Key
        public Guid QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; } = null!;
    }
}
