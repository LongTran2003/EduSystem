using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class StudentAnswer : BaseEntity<string, string, string>
    {
        [Key]
        public Guid StudentAnswerId { get; set; }

        [StringLength(500)]
        public string? AnswerText { get; set; } // Câu trả lời của học sinh

        public bool IsCorrect { get; set; } = false; // Đúng hay sai

        public int? Score { get; set; } // Điểm số đạt được

        public DateTime? SubmittedAt { get; set; } // Thời gian nộp bài

        public int? TimeSpent { get; set; } // Thời gian làm bài (giây)

        // Foreign Keys
        public Guid StudentId { get; set; }
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; } = null!;

        public Guid QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; } = null!;

        public Guid? AnswerId { get; set; } // Nullable vì có thể là câu trả lời tự luận
        [ForeignKey("AnswerId")]
        public virtual Answer? Answer { get; set; }
    }
}
