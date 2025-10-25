using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class StudentProgress
    {
        [Key]
        public Guid StudentProgressId { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = null!; // Trạng thái: NotStarted, InProgress, Completed, Failed

        public int? Score { get; set; } // Điểm số đạt được

        public int? MaxScore { get; set; } // Điểm số tối đa

        public DateTime? StartedAt { get; set; } // Thời gian bắt đầu

        public DateTime? CompletedAt { get; set; } // Thời gian hoàn thành

        public int? TimeSpent { get; set; } // Thời gian học (phút)

        public int? Attempts { get; set; } = 1; // Số lần thử

        // Foreign Keys
        public Guid StudentId { get; set; }
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; } = null!;

        public Guid LessonId { get; set; }
        [ForeignKey("LessonId")]
        public virtual Lesson Lesson { get; set; } = null!;
    }
}
