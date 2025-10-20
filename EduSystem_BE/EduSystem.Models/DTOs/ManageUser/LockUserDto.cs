using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTO.ManageUser
{
    public class LockUserDto
    {
        [Required]
        public string UserId { get; set; } = null!;
        public DateTimeOffset? LockoutEndDate { get; set; }
    }
}
