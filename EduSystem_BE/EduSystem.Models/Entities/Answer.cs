using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class Answer : BaseEntity<string, string, string>
    {
        [Key]
        public Guid AnswerId { get; set; }

        public Guid QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; } = null!;

        public string Content { get; set; } = null!;

        public bool IsCorrect { get; set; }

        public string? Explanation { get; set; }

        // Thêm thuộc tính ICollection dưới đây
        public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
    }
}
