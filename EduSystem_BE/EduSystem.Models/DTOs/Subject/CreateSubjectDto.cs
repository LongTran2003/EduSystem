namespace EduSystem.Models.DTO.Subject;

public class CreateSubjectDto
{
    public string SubjectName { get; set; }
    public string Description { get; set; }
    public string GradeLevel { get; set; }
    public string DifficultyLevel { get; set; }
    public string SubjectCode { get; set; }
}