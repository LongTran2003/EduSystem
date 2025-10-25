using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class Lesson : BaseEntity<string, string, string>    
    {
        [Key]
        public Guid LessonId { get; set; }

        [StringLength(200)]
        public string Title { get; set; } = null!; // Tiêu đề bài học

        [StringLength(1000)]
        public string? Description { get; set; } // Mô tả bài học

        [StringLength(20)]
        public string? GradeLevel { get; set; } // Cấp học: Grade 10, Grade 11, Grade 12

        [StringLength(20)]
        public string? DifficultyLevel { get; set; } // Mức độ khó: Beginner, Intermediate, Advanced

        [StringLength(50)]
        public string? LessonType { get; set; } // Loại bài học: Grammar, Vocabulary, Reading, Listening, Speaking, Writing

        public int? Duration { get; set; } // Thời lượng bài học (phút)

        // Foreign Keys
        public Guid SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; } = null!;

        public Guid TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; } = null!;

        // Navigation properties
        public virtual ICollection<LessonContent> LessonContents { get; set; } = new List<LessonContent>();
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual ICollection<StudentProgress> StudentProgresses { get; set; } = new List<StudentProgress>();
    }
}
