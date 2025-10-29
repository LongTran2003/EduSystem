using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTO.Quiz;

public class UpdateQuizDto
{
    public Guid QuizId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? GradeLevel { get; set; }
    public string? DifficultyLevel { get; set; }
    public int? TimeLimit { get; set; }
    public int? TotalQuestions { get; set; }
    public int? TotalPoints { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? MaxAttempts { get; set; }
    
    [Required]
    public Guid SubjectId { get; set; }

    [Required]
    public Guid TeacherId { get; set; }
}