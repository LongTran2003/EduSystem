using EduSystem.Models.DTO;
using EduSystem.Models.DTO.ManageUser;

namespace EduSystem.Services.IServices
{
    public interface IManageUserAccountService
    {
        Task<ResponseDto> LockUser(LockUserDto lockUserDto);
        Task<ResponseDto> UnlockUser(UnlockUserDto unLockUserDto);
    }
}
