using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.Entities
{
    public class Subject : BaseEntity<string, string, string>
    {
        [Key]
        public Guid SubjectId { get; set; }

        [StringLength(50)]
        public string SubjectName { get; set; } = null!; // Tên môn học: English, English Literature

        [StringLength(200)]
        public string? Description { get; set; } // Mô tả môn học

        [StringLength(20)]
        public string? GradeLevel { get; set; } // Cấp học: Grade 10, Grade 11, Grade 12

        [StringLength(20)]
        public string? DifficultyLevel { get; set; } // Mức độ khó: Beginner, Intermediate, Advanced

        [StringLength(100)]
        public string? SubjectCode { get; set; } // Mã môn học: ENG10, ENG11, ENG12

        // Navigation properties
        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    }
}
