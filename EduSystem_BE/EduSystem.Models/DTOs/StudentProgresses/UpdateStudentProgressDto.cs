using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTOs.StudentProgresses
{
    public class UpdateStudentProgressDto
    {
        [Required(ErrorMessage = "ProgressId is required")]
        public Guid ProgressId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "CompletedLessons must be non-negative")]
        public int? CompletedLessons { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "TotalLessons must be greater than 0")]
        public int? TotalLessons { get; set; }

        [Range(0, 100, ErrorMessage = "AverageScore must be between 0 and 100")]
        public decimal? AverageScore { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "TotalTimeSpent must be non-negative")]
        public int? TotalTimeSpent { get; set; }
    }
}
