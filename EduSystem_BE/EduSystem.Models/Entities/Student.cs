using EduSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class Student
    {
        [Key]
        public Guid StudentId { get; set; }

        public string UserId { get; set; } = null!;
        [ForeignKey("UserId")] public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        [StringLength(11)]
        public string StudentCode { get; set; } = null!;

        public StudentStatus Status { get; set; } = StudentStatus.Active;

        [StringLength(100)]
        public string? Grade { get; set; } // Khối lớp, ví dụ: Khối 10, Khối 11, v.v.

        [StringLength(100)]
        public string? School { get; set; } // Trường học

        // Thêm các thuộc tính ICollection dưới đây
        public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
        public virtual ICollection<StudentProgress> StudentProgresses { get; set; } = new List<StudentProgress>();

    }
}
