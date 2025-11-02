using EduSystem.Utilities.Contants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class Unit : BaseEntity<string, string, string>
    {
        [Key]
        public Guid UnitId { get; set; }

        public Guid TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public virtual Teacher? Teacher { get; set; } = null!;

        [StringLength(200)]
        public string UnitName { get; set; } = null!;

        [StringLength(20)]
        public string? EnglishLevel { get; set; }

        public string? Description { get; set; }

        public string? LearningObjectives { get; set; }

        public int OrderIndex { get; set; } // Thứ tự Unit (Unit 1, Unit 2)

        public string Status { get; set; } = StaticOperationStatus.BaseEntity.Active;

        // Navigation properties
        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    }
}
