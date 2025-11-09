namespace EduSystem.Models.DTOs.MatrixDetail
{
    public class MatrixDetailDto
    {
        public Guid DetailId { get; set; }
        public Guid MatrixId { get; set; }
        public int Level { get; set; }
        public int QuestionType { get; set; }
        public string? SkillType { get; set; }
        public int QuestionCount { get; set; }
        public decimal ScorePerQuestion { get; set; }
        public string Status { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
