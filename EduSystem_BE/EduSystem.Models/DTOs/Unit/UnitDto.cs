namespace EduSystem.Models.DTO.Unit;

public class UnitDto
{
    public Guid UnitId { get; set; }
    public string TeacherName { get; set; }
    public string UnitName { get; set; }
    public string EnglishLevel { get; set; }
    public string Description { get; set; }
    public string LearningObjectives { get; set; }
    public int OrderIndex { get; set; }
    public string? Status { get; set; }
    public string? CreateBy { get; set; }
    public string? CreateTime { get; set; }
    public string? UpdateBy { get; set; }
    public string? UpdateTime { get; set; }
    
}