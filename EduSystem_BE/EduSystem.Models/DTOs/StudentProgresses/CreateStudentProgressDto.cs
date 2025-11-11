using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTOs.StudentProgresses
{
    public class CreateStudentProgressDto
    {
        [Required(ErrorMessage = "StudentId is required")]
        public Guid StudentId { get; set; }

        [Required(ErrorMessage = "UnitId is required")]
        public Guid UnitId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "CompletedLessons must be non-negative")]
        public int CompletedLessons { get; set; } = 0;

        [Required(ErrorMessage = "TotalLessons is required")]
        [Range(1, int.MaxValue, ErrorMessage = "TotalLessons must be greater than 0")]
        public int TotalLessons { get; set; }

        [Range(0, 100, ErrorMessage = "AverageScore must be between 0 and 100")]
        public decimal AverageScore { get; set; } = 0;

        [Range(0, int.MaxValue, ErrorMessage = "TotalTimeSpent must be non-negative")]
        public int TotalTimeSpent { get; set; } = 0;
    }
}
