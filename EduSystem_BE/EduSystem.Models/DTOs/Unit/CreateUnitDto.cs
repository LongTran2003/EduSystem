namespace EduSystem.Models.DTO.Unit;

public class CreateUnitDto
{
    public Guid TeacherId { get; set; }
    public string UnitName { get; set; } = null!;
    public string? EnglishLevel { get; set; }
    public string? Description { get; set; }
    public string? LearningObjectives { get; set; }
    public int OrderIndex { get; set; }
}