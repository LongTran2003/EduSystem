using EduSystem.Models.DTOs.Answer;

namespace EduSystem.Models.DTO.Question;

public class QuestionDto
{
    public Guid QuestionId { get; set; }
    public Guid TeacherId { get; set; }
    public string? TeacherName { get; set; }
    public string? Content { get; set; }
    public string? QuestionType { get; set; }
    public string? Level { get; set; }
    public string? SkillType { get; set; }
    public string? EnglishLevel { get; set; }
    public decimal Score { get; set; }
    public string Status { get; set; } = null!;
    public string? CreatedBy { get; set; }
    public DateTime CreatedTime { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedTime { get; set; }
    public List<AnswerDto>? Answers { get; set; }
}