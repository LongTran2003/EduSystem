namespace EduSystem.Models.DTO.Question;

public class QuestionDto
{
    public Guid QuestionId { get; set; }
    public string TeacherName { get; set; }
    public string Content { get; set; }
    public string? QuestionType { get; set; }
    public string? Level { get; set; }
    public string? SkillType { get; set; }
    public string? EnglishLevel { get; set; }
    public decimal Score { get; set; }
}