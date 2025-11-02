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
    
}