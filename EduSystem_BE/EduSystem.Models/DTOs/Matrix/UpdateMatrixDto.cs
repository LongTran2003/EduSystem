namespace EduSystem.Models.DTOs.Matrix
{
    public class UpdateMatrixDto
    {
        public Guid MatrixId { get; set; }
        public string Name { get; set; } = null!;
        public string? EnglishLevel { get; set; }
        public string? SkillFocus { get; set; }
        public string? Description { get; set; }
    }
}
