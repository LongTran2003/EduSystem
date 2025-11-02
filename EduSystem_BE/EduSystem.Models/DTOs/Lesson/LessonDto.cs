namespace EduSystem.Models.DTO.Lesson;

public class LessonDto
{
    public Guid LessonId { get; set; } 
    public string? UnitName { get; set; }
    public string LessonName { get; set; } = null!;
    public string? Skill { get; set; }
    public string? Content { get; set; }
    public int? Duration { get; set; }
    public int OrderIndex { get; set; }
    public string? Status { get; set; }
    public string? CreateBy { get; set; }
    public string? CreateTime { get; set; }
    public string? UpdateBy { get; set; }
    public string? UpdateTime { get; set; }

}