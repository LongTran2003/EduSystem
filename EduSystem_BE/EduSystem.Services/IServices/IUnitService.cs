using System.Security.Claims;
using EduSystem.Models.DTO;
using EduSystem.Models.DTO.Unit;
using EduSystem.Models.DTOs.Teacher;

namespace EduSystem.Services.IServices;

public interface IUnitService
{
    Task<ResponseDto> CreateUnit(ClaimsPrincipal user, CreateUnitDto createUnitDto);
    Task<ResponseDto> UpdateUnit(ClaimsPrincipal user, UpdateUnitDto updateUnitDto);
    Task<ResponseDto> GetUnitDetailsById(ClaimsPrincipal user, Guid unitId);
    Task<ResponseDto> GetAllUnits
    (
        ClaimsPrincipal user,
        int pageNumber = 1,
        int pageSize = 10,
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null
    );
    Task<ResponseDto> DeleteUnit(ClaimsPrincipal user, Guid unitId);
}