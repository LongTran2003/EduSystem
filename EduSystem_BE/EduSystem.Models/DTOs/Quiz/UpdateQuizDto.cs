using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTO.Quiz;

public class UpdateQuizDto
{
    public Guid QuizId { get; set; }
    public Guid MatrixId { get; set; }
    public string QuizName { get; set; } = null!;
    public string? EnglishLevel { get; set; }
    public string? Skill { get; set; }
    public string? Description { get; set; }
    public int Duration { get; set; }
    public decimal PassingScore { get; set; }
}