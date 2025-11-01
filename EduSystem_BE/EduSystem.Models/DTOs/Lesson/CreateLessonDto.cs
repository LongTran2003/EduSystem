using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTO.Lesson;

public class CreateLessonDto
{
    public Guid UnitId { get; set; }
    [StringLength(200)]
    public string LessonName { get; set; } = null!;

    [StringLength(50)]
    public string? Skill { get; set; } // Reading, Writing, Listening, Speaking

    public string? Content { get; set; }

    public int? Duration { get; set; } // Minutes

    public int OrderIndex { get; set; } = 0; // Default value

}   