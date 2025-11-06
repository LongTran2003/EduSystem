using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTOs.Matrix
{
    public class CreateMatrixDto
    {
        public string Name { get; set; } = null!;

        public string? EnglishLevel { get; set; }

        public string? SkillFocus { get; set; }

        public string? Description { get; set; }
    }
}
