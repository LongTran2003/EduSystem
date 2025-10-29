using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTO.Question;

public class CreateQuestionDto
{
    public string QuestionText { get; set; }
    public string QuestionType { get; set; }
    public string? DifficultyLevel { get; set; }
    public string? GradeLevel { get; set; }
    public int? Points { get; set; }
    public int? TimeLimit { get; set; }
    
    [Required]
    public Guid? SubjectId { get; set; }
    
    [Required]
    public Guid? LessonId { get; set; }

    [Required]
    public Guid TeacherId { get; set; }
}