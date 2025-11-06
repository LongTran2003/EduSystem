using EduSystem.Models.DTO;
using EduSystem.Models.DTO.Question;
using EduSystem.Models.DTOs.Matrix;
using System.Security.Claims;

namespace EduSystem.Services.IServices
{
    public interface IMatrixService
    {
        Task<ResponseDto> CreateMatrix(ClaimsPrincipal user, CreateMatrixDto createMatrixDto);
        Task<ResponseDto> UpdateMatrix(ClaimsPrincipal user, UpdateMatrixDto updateMatrixDto);
        Task<ResponseDto> GetAllMatrices(
            ClaimsPrincipal user,
            int pageNumber = 1,
            int pageSize = 10,
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null);
        Task<ResponseDto> GetMatrixById(ClaimsPrincipal user, Guid matrixId);
        Task<ResponseDto> DeleteMatrix(ClaimsPrincipal user, Guid matrixId);
    }
}
