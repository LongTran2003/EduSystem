namespace EduSystem.Models.DTOs.Matrix
{
    public class MatrixDto
    {
        public Guid MatrixId { get; set; }
        public string Name { get; set; } = null!;
        public string? EnglishLevel { get; set; }
        public string? SkillFocus { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedTime { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UpdatedTime { get; set; }
    }
}
