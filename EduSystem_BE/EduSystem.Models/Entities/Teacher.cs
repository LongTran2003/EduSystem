using EduSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class Teacher : BaseEntity<string, string, string>
    {
        [Key]
        public Guid TeacherId { get; set; }
        public string UserId { get; set; } = null!;
        [ForeignKey("UserId")] public virtual ApplicationUser ApplicationUser { get; set; } = null!;
        [StringLength(8)] public string TeacherCode { get; set; } = null!;
        public TeacherStatus Status { get; set; } = TeacherStatus.Active;
        [StringLength(100)]
        public string? Department { get; set; } // Khoa, ví dụ: Khoa Khoa học Tự nhiên, Khoa Xã hội Nhân văn, v.v.

        [StringLength(100)]
        public string? Specialization { get; set; } // Chuyên môn, ví dụ: Toán học, Vật lý, v.v.

        [StringLength(50)]
        public string? Position { get; set; } // Giáo viên, Trưởng khoa, Phó khoa, v.v.

        public DateTime? HireDate { get; set; } // Ngày tuyển dụng

        [StringLength(20)]
        public string? Degree { get; set; } // Thạc sĩ, Tiến sĩ, v.v.

        [StringLength(200)]
        public string? Bio { get; set; } // Tiểu sử ngắn gọn về giáo viên

        // Thông tin chuyên sâu
        [StringLength(20)]
        public string? EnglishProficiency { get; set; } // Trình độ tiếng Anh: Native, C2, C1

        [StringLength(100)]
        public string? Certifications { get; set; } // Chứng chỉ: TESOL, TEFL, IELTS, TOEFL

        [StringLength(50)]
        public string? TeachingExperience { get; set; } // Kinh nghiệm giảng dạy: 5+ years, 10+ years

        // Thông tin liên hệ
        [StringLength(20)]
        public string? OfficePhone { get; set; } // Số điện thoại văn phòng
    }
}
