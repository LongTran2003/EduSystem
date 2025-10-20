using System.ComponentModel.DataAnnotations;

namespace EduSystem.Models.DTO.Authentication
{
    public class UpdateUserProfileDto
    {
        public string FullName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        //public string IdentityCard { get; set; } = null!;
        [Phone] public string PhoneNumber { get; set; } = null!;
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string? ImageUrl { get; set; }
    }
}
