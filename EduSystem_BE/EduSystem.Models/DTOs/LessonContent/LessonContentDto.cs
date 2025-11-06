namespace EduSystem.Models.DTOs.LessonContent;

public class LessonContentDto
{
    public Guid LessonContentId { get; set; }
    public Guid LessonId { get; set; }
    public string? ResourceType { get; set; }
    public string ResourceUrl { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? CreatedBy { get; set; }
    public DateTime CreatedTime { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedTime { get; set; }
}