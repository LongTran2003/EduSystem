using EduSystem.Utilities.Contants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class Quiz : BaseEntity<string, string, string>
    {
        [Key]
        public Guid QuizId { get; set; }

        public Guid MatrixId { get; set; }
        [ForeignKey("MatrixId")]
        public virtual Matrix Matrix { get; set; } = null!;

        public Guid TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; } = null!;

        [StringLength(200)]
        public string QuizName { get; set; } = null!;

        [StringLength(20)]
        public string? EnglishLevel { get; set; }

        [StringLength(50)]
        public string? Skill { get; set; }

        public string? Description { get; set; }

        public int Duration { get; set; } // Minutes

        [Column(TypeName = "decimal(5,2)")]
        public decimal PassingScore { get; set; }

        public string Status { get; set; } = StaticOperationStatus.BaseEntity.Active;

        // Navigation properties
        public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
        public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
    }
}
