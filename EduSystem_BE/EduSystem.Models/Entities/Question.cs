using EduSystem.Utilities.Contants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class Question : BaseEntity<string, string, string>
    {
        [Key]
        public Guid QuestionId { get; set; }

        public Guid TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string? QuestionType { get; set; }

        public string? Level { get; set; } // 1:Easy, 2:Medium, 3:Hard

        [StringLength(50)]
        public string? SkillType { get; set; }

        [StringLength(20)]
        public string? EnglishLevel { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Score { get; set; }

        public string Status { get; set; } = StaticOperationStatus.BaseEntity.Active;

        // Navigation properties
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
        public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
    }
}
