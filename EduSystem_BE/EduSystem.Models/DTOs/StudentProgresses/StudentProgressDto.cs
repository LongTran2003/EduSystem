namespace EduSystem.Models.DTOs.StudentProgresses
{
    public class StudentProgressDto
    {
        public Guid ProgressId { get; set; }
        public Guid StudentId { get; set; }
        public string? StudentName { get; set; }

        public Guid UnitId { get; set; }
        public string? UnitName { get; set; }

        public int CompletedLessons { get; set; }
        public int TotalLessons { get; set; }
        public decimal ProgressPercentage { get; set; }

        public decimal AverageScore { get; set; }
        public int TotalTimeSpent { get; set; }
        public DateTime LastAccessDate { get; set; }

        public string Status { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
