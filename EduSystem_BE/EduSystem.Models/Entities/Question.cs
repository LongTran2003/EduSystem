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

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = null!; 

        [Required]
        public string Content { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string QuestionType { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string Level { get; set; } = "Easy"; 
        
        [StringLength(50)]
        public string? SkillType { get; set; }

        [StringLength(20)]
        public string? EnglishLevel { get; set; }

        [Required]
        [Range(0.0, 10.0)]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Score { get; set; }

        public string Status { get; set; } = StaticOperationStatus.BaseEntity.Active;

        // Navigation properties
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
        public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
    }
}
