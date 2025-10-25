using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class Quiz : BaseEntity<string, string, string>
    {
        [Key]
        public Guid QuizId { get; set; }

        [StringLength(200)]
        public string Title { get; set; } = null!; // Tiêu đề bài kiểm tra

        [StringLength(500)]
        public string? Description { get; set; } // Mô tả bài kiểm tra

        [StringLength(20)]
        public string? GradeLevel { get; set; } // Cấp học: Grade 10, Grade 11, Grade 12

        [StringLength(20)]
        public string? DifficultyLevel { get; set; } // Mức độ khó: Easy, Medium, Hard

        public int? TimeLimit { get; set; } // Thời gian làm bài (phút)

        public int? TotalQuestions { get; set; } // Tổng số câu hỏi

        public int? TotalPoints { get; set; } // Tổng điểm

        public bool IsPublished { get; set; } = false; // Trạng thái xuất bản

        public DateTime? StartDate { get; set; } // Ngày bắt đầu

        public DateTime? EndDate { get; set; } // Ngày kết thúc

        public int? MaxAttempts { get; set; } = 1; // Số lần thử tối đa

        // Foreign Keys
        public Guid SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; } = null!;

        public Guid TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; } = null!;

        // Navigation properties
        public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
        public virtual ICollection<StudentQuizAttempt> StudentQuizAttempts { get; set; } = new List<StudentQuizAttempt>();
    }
}
