using EduSystem.Models.Enums;

namespace EduSystem.Models.DTOs.Teacher
{
    public class GetTeacherDto
    {
        public string TeacherId { get; set; } = null!;
        public string TeacherName { get; set; } = null!;
        public string TeacherEmail { get; set; } = null!;
        public DateTime? TeacherDOB { get; set; }
        public string? Gender { get; set; }
        public string Address { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public TeacherStatus Status { get; set; }
        public string TeacherCode { get; set; } = null!;
        public string? Specialization { get; set; }
        public string? TeachingExperience { get; set; }
        public decimal? Rating { get; set; }


    }
}
