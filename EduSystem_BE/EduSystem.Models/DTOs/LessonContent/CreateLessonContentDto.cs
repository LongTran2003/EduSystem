using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTOs.LessonContent
{
    public class CreateLessonContentDto
    {
        public Guid LessonId { get; set; }
        public string? ResourceType { get; set; }
        public string ResourceUrl { get; set; }
        public string? Description { get; set; }
    }
}
