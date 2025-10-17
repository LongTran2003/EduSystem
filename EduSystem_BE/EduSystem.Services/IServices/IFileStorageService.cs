using EduSystem.Models.DTO;
using EduSystem.Models.DTO.FileStorage;
using System.Security.Claims;

namespace EduSystem.Services.IServices
{
    public interface IFileStorageService
    {
        Task<ResponseDto> UploadAvatarImage(UploadFileDto uploadFileDto, ClaimsPrincipal user);
        //Task<ResponseDto> UploadProductImage(ClaimsPrincipal user, UploadFileDto uploadFileDto);
        //Task<ResponseDto> UploadProductComboImage(ClaimsPrincipal user, UploadFileDto uploadFileDto);
        //Task<ResponseDto> UploadMovieImage(ClaimsPrincipal user, UploadFileDto uploadFileDto);
        //Task<ResponseDto> UploadActorImage(ClaimsPrincipal user, UploadFileDto uploadFileDto);
    }
}
