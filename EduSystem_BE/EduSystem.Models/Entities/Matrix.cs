using EduSystem.Utilities.Contants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class Matrix : BaseEntity<string, string, string>
    {
        [Key]
        public Guid MatrixId { get; set; }

        public Guid TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; } = null!;

        [StringLength(200)]
        public string Name { get; set; } = null!;

        [StringLength(20)]
        public string? EnglishLevel { get; set; }

        [StringLength(50)]
        public string? SkillFocus { get; set; }

        public string? Description { get; set; }

        public string Status { get; set; } = StaticOperationStatus.BaseEntity.Active;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<MatrixDetail> MatrixDetails { get; set; } = new List<MatrixDetail>();
        public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    }
}
