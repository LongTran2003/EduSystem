using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class StudentProgress
    {
        [Key]
        public Guid StudentId { get; set; }
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; } = null!;

        public Guid LessonId { get; set; }
        [ForeignKey("LessonId")]
        public virtual Lesson Lesson { get; set; } = null!;

        public int CompletionStatus { get; set; } = 0;

        public DateTime? LastAccessDate { get; set; }

        public int TimeSpent { get; set; } = 0; // Minutes

        public string? Notes { get; set; }
    }
}
