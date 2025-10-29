using System.ComponentModel.DataAnnotations;
using EduSystem.Models.Enums;

namespace EduSystem.Models.DTOs.Teacher;

public class UpdateTeacherStatusDto
{
    [Required]
    public Guid TeacherId { get; set; }

    [Required]
    public TeacherStatus Status { get; set; }
}