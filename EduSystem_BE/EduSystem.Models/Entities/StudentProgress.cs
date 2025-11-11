using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class StudentProgress
    {
        [Key]
        public Guid ProgressId { get; set; }

        public Guid StudentId { get; set; }
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; } = null!;

        public Guid UnitId { get; set; }
        [ForeignKey("UnitId")]
        public virtual Unit Unit { get; set; } = null!;

        [Range(0, int.MaxValue)]
        public int CompletedLessons { get; set; } = 0;

        [Range(1, int.MaxValue)]
        public int TotalLessons { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        [Range(0, 100)]
        public decimal AverageScore { get; set; } = 0;

        public DateTime LastAccessDate { get; set; }

        [Range(0, int.MaxValue)]
        public int TotalTimeSpent { get; set; } = 0; // in minutes
    }
}
