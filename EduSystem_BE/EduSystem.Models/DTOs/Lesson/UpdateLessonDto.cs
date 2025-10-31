namespace EduSystem.Models.DTO.Lesson;

public class UpdateLessonDto
{
    public Guid LessonId { get; set; }
    public Guid SubjectId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? GradeLevel { get; set; }
    public string? DifficultyLevel { get; set; }
    public string? LessonType { get; set; }
    public int? Duration  { get; set; }

}