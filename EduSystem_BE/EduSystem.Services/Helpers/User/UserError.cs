using System.Security.Claims;

namespace EduSystem.Services.Helpers.User
{
    public class UserError
    {
        public static bool NotExists(ClaimsPrincipal User)
        {
            return User is null || (
                User.FindFirstValue(ClaimTypes.NameIdentifier) is null &&
                User.FindFirstValue("FullName") is null);
        }
    }
}
