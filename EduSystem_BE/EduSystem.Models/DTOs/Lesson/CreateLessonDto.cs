namespace EduSystem.Models.DTO.Lesson;

public class CreateLessonDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? GradeLevel { get; set; }
    public string? DifficultyLevel { get; set; }
    public string? LessonType { get; set; }
    public int? Duration  { get; set; }
}