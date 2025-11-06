using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTOs.LessonContent
{
    public class UpdateLessonContentDto
    {
        public Guid LessonContentId { get; set; }
        public Guid LessonId { get; set; }
        public string? ResourceType { get; set; }
        public string ResourceUrl { get; set; }
        public string? Description { get; set; }
    }
}
