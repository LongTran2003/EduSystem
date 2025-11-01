using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.Entities
{
    public class MatrixDetail : BaseEntity<string, string, string>
    {
        [Key]
        public Guid DetailId { get; set; }

        public Guid MatrixId { get; set; }
        [ForeignKey("MatrixId")]
        public virtual Matrix Matrix { get; set; } = null!;

        public int Level { get; set; } // 1:Easy, 2:Medium, 3:Hard

        public int QuestionType { get; set; } // 1:MultipleChoice, 2:Essay, etc.

        [StringLength(50)]
        public string? SkillType { get; set; } // Grammar, Vocabulary, Reading, etc.

        public int QuestionCount { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal ScorePerQuestion { get; set; }

    }
}
