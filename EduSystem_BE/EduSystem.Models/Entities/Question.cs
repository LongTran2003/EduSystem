using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class Question : BaseEntity<string, string, string>
    {
        [Key]
        public Guid QuestionId { get; set; }

        [StringLength(1000)]
        public string QuestionText { get; set; } = null!; // Nội dung câu hỏi

        [StringLength(50)]
        public string QuestionType { get; set; } = null!; // Loại câu hỏi: MultipleChoice, TrueFalse, FillBlank, Essay

        [StringLength(20)]
        public string? DifficultyLevel { get; set; } // Mức độ khó: Easy, Medium, Hard

        [StringLength(20)]
        public string? GradeLevel { get; set; } // Cấp học: Grade 10, Grade 11, Grade 12

        public int? Points { get; set; } = 1; // Điểm số câu hỏi

        public int? TimeLimit { get; set; } // Thời gian làm bài (giây)


        // Foreign Keys
        public Guid? SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject? Subject { get; set; }

        public Guid? LessonId { get; set; }
        [ForeignKey("LessonId")]
        public virtual Lesson? Lesson { get; set; }

        public Guid TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; } = null!;

        // Navigation properties
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
        public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
    }
}
