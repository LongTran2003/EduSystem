using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class StudentQuizAttempt : BaseEntity<string, string, string>
    {
        [Key]
        public Guid StudentQuizAttemptId { get; set; }

        public int AttemptNumber { get; set; } = 1; // Số lần thử

        public int? Score { get; set; } // Điểm số đạt được

        public int? MaxScore { get; set; } // Điểm số tối đa

        public DateTime? StartedAt { get; set; } // Thời gian bắt đầu

        public DateTime? CompletedAt { get; set; } // Thời gian hoàn thành

        public int? TimeSpent { get; set; } // Thời gian làm bài (phút)

        [StringLength(20)]
        public string Status { get; set; } = null!; // Trạng thái: InProgress, Completed, Abandoned

        // Foreign Keys
        public Guid StudentId { get; set; }
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; } = null!;

        public Guid QuizId { get; set; }
        [ForeignKey("QuizId")]
        public virtual Quiz Quiz { get; set; } = null!;

        // Navigation properties
        public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
    }
}
