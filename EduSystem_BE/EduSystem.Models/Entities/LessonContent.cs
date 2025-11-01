using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class LessonContent : BaseEntity<string, string, string>
    {
        [Key]
        public Guid LessonContentId { get; set; }

        public Guid LessonId { get; set; }
        [ForeignKey("LessonId")]
        public virtual Lesson Lesson { get; set; } = null!;

        [StringLength(50)]
        public string? ResourceType { get; set; } // Video, Audio, Document, Worksheet

        [StringLength(255)]
        public string ResourceUrl { get; set; } = null!;

        public string? Description { get; set; }

    }
}
