using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTO.Lesson;

public class UpdateLessonDto
{
    public Guid LessonId { get; set; }
    public Guid UnitId { get; set; }
    [StringLength(200)]
    public string LessonName { get; set; } = null!;
    [StringLength(50)]
    public string? Skill { get; set; }
    public string? Content { get; set; }
    public int? Duration { get; set; }
    public int OrderIndex { get; set; }
    public string? Status { get; set; }

}