namespace EduSystem.Models.DTO.Authentication
{
    public class SignInResponseDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
