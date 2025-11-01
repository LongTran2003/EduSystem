using EduSystem.Utilities.Contants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class Lesson : BaseEntity<string, string, string>    
    {
        [Key]
        public Guid LessonId { get; set; }

        [StringLength(200)]
        public Guid UnitId { get; set; }
        [ForeignKey("UnitId")]
        public virtual Unit Unit { get; set; } = null!;

        [StringLength(200)]
        public string LessonName { get; set; } = null!;

        [StringLength(50)]
        public string? Skill { get; set; } // Reading, Writing, Listening, Speaking

        public string? Content { get; set; }

        public int? Duration { get; set; } // Minutes

        public int OrderIndex { get; set; } // Thứ tự của bài học (Bài 1, Bài 2)

        public string Status { get; set; } = StaticOperationStatus.BaseEntity.Active;

        // Navigation properties
        public virtual ICollection<LessonContent> LessonContents { get; set; } = new List<LessonContent>();
        public virtual ICollection<StudentProgress> StudentProgresses { get; set; } = new List<StudentProgress>();
    }
}
