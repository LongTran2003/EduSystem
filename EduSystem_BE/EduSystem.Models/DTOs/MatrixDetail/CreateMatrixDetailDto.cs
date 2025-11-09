namespace EduSystem.Models.DTOs.MatrixDetail
{
    public class CreateMatrixDetailDto
    {
        public Guid MatrixId { get; set; }
        public int Level { get; set; }
        public int QuestionType { get; set; }
        public string? SkillType { get; set; }
        public int QuestionCount { get; set; }
        public decimal ScorePerQuestion { get; set; }
    }
}
