using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class QuizQuestion : BaseEntity<string, string, string>
    {
        [Key]
        public Guid QuizQuestionId { get; set; }

        public int? OrderIndex { get; set; } // Thứ tự câu hỏi trong bài kiểm tra

        public int? Points { get; set; } = 1; // Điểm số câu hỏi

        // Foreign Keys
        public Guid QuizId { get; set; }
        [ForeignKey("QuizId")]
        public virtual Quiz Quiz { get; set; } = null!;

        public Guid QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; } = null!;
    }
}
