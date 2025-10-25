using EduSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class Student : BaseEntity<string, string, string>
    {
        [Key]
        public Guid StudentId { get; set; }

        public string UserId { get; set; } = null!;
        [ForeignKey("UserId")] public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        [StringLength(10)]
        public string StudentCode { get; set; } = null!;

        public StudentStatus Status { get; set; } = StudentStatus.Active;

        [StringLength(50)]
        public string? Class { get; set; } // lớp học, ví dụ: 10A1, 11B2, v.v.

        [StringLength(100)]
        public string? Grade { get; set; } // Khối lớp, ví dụ: Khối 10, Khối 11, v.v.

        [StringLength(100)]
        public string? School { get; set; } // Khoa, ví dụ: Khoa Khoa học Tự nhiên, Khoa Xã hội Nhân văn, v.v.

        public DateTime? EnrollmentDate { get; set; } // Ngày nhập học

        public DateTime? GraduationDate { get; set; } // Ngày tốt nghiệp dự kiến
    }
}
