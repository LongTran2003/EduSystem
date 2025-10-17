using EduSystem.Models.DTO;
using EduSystem.Models.DTO.Authentication;
using EduSystem.Models.DTO.Email;
using System.Security.Claims;

namespace EduSystem.Services.IServices
{
    public interface IAuthService
    {
        Task<ResponseDto> SignUpStudent(SignUpStudentDto signUpStudentDto);
        Task<ResponseDto> SignUpTeacher(SignUpTeacherDto signUpTeacherDto);
        Task<ResponseDto> SignIn(SignInDto signInDto);
        Task<ResponseDto> SendVerifyEmail(EmailDto emailDto);
        Task<ResponseDto> VerifyEmail(VerifyEmailDto verifyEmailDto);
        Task<ResponseDto> ForgotPassword(EmailDto forgotPasswordDto);
        Task<ResponseDto> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<ResponseDto> ChangePassword(ChangePasswordDto changePasswordDto, ClaimsPrincipal User);
        Task<ResponseDto> SendOTP(EmailDto sendOTPDto); 
        Task<ResponseDto> FetchUserByToken(ClaimsPrincipal user);
        Task<ResponseDto> UpdateUserProfile(ClaimsPrincipal userPrincipal, UpdateUserProfileDto updateUserProfileDto);
        Task<ResponseDto> RefreshAccessToken(RefreshTokenDto refreshTokenDto);
    }
}
