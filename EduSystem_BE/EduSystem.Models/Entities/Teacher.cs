using EduSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class Teacher
    {
        [Key]
        public Guid TeacherId { get; set; }
        public string UserId { get; set; } = null!;
        [ForeignKey("UserId")] public virtual ApplicationUser ApplicationUser { get; set; } = null!;
        [StringLength(8)] public string TeacherCode { get; set; } = null!;
        public TeacherStatus Status { get; set; } = TeacherStatus.Active;

        [StringLength(100)]
        public string? Specialization { get; set; } // Chuyên môn, ví dụ: Toán học, Vật lý, v.v.

        [StringLength(50)] public int TeachingExperience { get; set; } = 0; // Kinh nghiệm giảng dạy: 5+ years, 10+ years

        [Column(TypeName = "decimal(3,2)")]
        public decimal? Rating { get; set; }
    }
}
