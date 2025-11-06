using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTO.Question;

public class CreateQuestionDto
{
    public string Title { get; set; } = null!;
    public string? Content { get; set; }
    public string? QuestionType { get; set; }
    public string? Level { get; set; } = "Easy";
    public string? SkillType { get; set; }
    public string? EnglishLevel { get; set; }
    
    [Range(0.0, 10.0)]
    public decimal Score { get; set; }
    /*public List<CreateAnswerDto>? Answers { get; set; }*/
}