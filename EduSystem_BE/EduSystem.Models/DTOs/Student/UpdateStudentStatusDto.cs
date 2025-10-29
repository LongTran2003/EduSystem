using System.ComponentModel.DataAnnotations;
using EduSystem.Models.Enums;

namespace EduSystem.Models.DTO.Student;

public class UpdateStudentStatusDto
{
    [Required]
    public Guid StudentId { get; set; }

    [Required]
    public StudentStatus Status { get; set; }
}