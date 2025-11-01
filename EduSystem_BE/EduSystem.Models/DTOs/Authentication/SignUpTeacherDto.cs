using EduSystem.Utilities.ValidationAttribute;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSystem.Models.DTO.Authentication
{
    public class SignUpTeacherDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Password]
        public string Password { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [ConfirmPassword("Password")]
        [NotMapped]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;
        [Required]
        public string Gender { get; set; } = null!;

        [Required]
        public DateTime BirthDate { get; set; }

        [StringLength(100)]
        public string? Specialization { get; set; }

        [StringLength(50)]
        public int TeachingExperience { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
        [Column(TypeName = "decimal(3,2)")]
        public decimal? Rating { get; set; }
    }
}
