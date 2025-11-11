using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class QuizQuestion : BaseEntity<string, string, string>
    {
        [Key]
        public Guid QuizId { get; set; }
        [ForeignKey("QuizId")]
        public virtual Quiz Quiz { get; set; } = null!;

        public Guid QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue)]
        public int QuestionOrder { get; set; }
    }
}
