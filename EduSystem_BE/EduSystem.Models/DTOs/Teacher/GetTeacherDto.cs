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
        public string OfficePhone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? ImageUrl { get; set; } = null!;
        public TeacherStatus Status { get; set; }
        public string TeacherCode { get; set; } = null!;
        public string? Department { get; set; }
        public string? Specialization { get; set; }
        public string? Position { get; set; }
        public DateTime? HireDate { get; set; }
        public string? Degree { get; set; }
        public string? Bio { get; set; }



    }
}
