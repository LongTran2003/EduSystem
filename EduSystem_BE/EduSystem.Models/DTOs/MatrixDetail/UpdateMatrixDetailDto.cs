namespace EduSystem.Models.DTOs.MatrixDetail
{
    public class UpdateMatrixDetailDto
    {
        public Guid DetailId { get; set; }
        public int? Level { get; set; }
        public int? QuestionType { get; set; }
        public string? SkillType { get; set; }
        public int? QuestionCount { get; set; }
        public decimal? ScorePerQuestion { get; set; }
        public string? Status { get; set; }
    }
}
