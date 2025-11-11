using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTOs.StudentProgresses
{
    public class UpdateProgressDto
    {
        [Required(ErrorMessage = "StudentId is required")]
        public Guid StudentId { get; set; }

        [Required(ErrorMessage = "UnitId is required")]
        public Guid UnitId { get; set; }

        [Required(ErrorMessage = "CompletedLessonId is required")]
        public Guid CompletedLessonId { get; set; }

        [Range(0, 100, ErrorMessage = "LessonScore must be between 0 and 100")]
        public decimal? LessonScore { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "TimeSpent must be non-negative")]
        public int? TimeSpent { get; set; }
    }
}
