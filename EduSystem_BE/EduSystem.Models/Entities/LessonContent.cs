using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class LessonContent : BaseEntity<string, string, string>
    {
        [Key]
        public Guid LessonContentId { get; set; }

        [StringLength(200)]
        public string Title { get; set; } = null!; // Tiêu đề nội dung

        [StringLength(50)]
        public string ContentType { get; set; } = null!; // Loại nội dung: Text, Image, Video, Audio, Document

        public string? Content { get; set; } // Nội dung chính (HTML, Markdown)

        [StringLength(500)]
        public string? FileUrl { get; set; } // URL file đính kèm

        [StringLength(200)]
        public string? FileName { get; set; } // Tên file

        public int? FileSize { get; set; } // Kích thước file (bytes)

        public bool IsRequired { get; set; } = true; // Bắt buộc hay không

        // Foreign Key
        public Guid LessonId { get; set; }
        [ForeignKey("LessonId")]
        public virtual Lesson Lesson { get; set; } = null!;

    }
}
