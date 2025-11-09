using EduSystem.Models.DTO;
using EduSystem.Models.DTOs.MatrixDetail;
using System.Security.Claims;

namespace EduSystem.Services.IServices
{
    public interface IMatrixDetailService
    {
        Task<ResponseDto> CreateMatrixDetail(ClaimsPrincipal user, CreateMatrixDetailDto createMatrixDetailDto);
        Task<ResponseDto> UpdateMatrixDetail(ClaimsPrincipal user, UpdateMatrixDetailDto updateMatrixDetailDto);
        Task<ResponseDto> GetAllMatrixDetails(
            ClaimsPrincipal user, 
            int pageNumber = 1, 
            int pageSize = 10,
            string? filterOn = null, 
            string? filterQuery = null, 
            string? sortBy = null);
        Task<ResponseDto> GetMatrixDetailById(ClaimsPrincipal user, Guid detailId);
        Task<ResponseDto> DeleteMatrixDetail(ClaimsPrincipal user, Guid detailId);

    }
}
