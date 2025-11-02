using System.Security.Claims;
using AutoMapper;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.DTO;
using EduSystem.Models.DTO.Unit;
using EduSystem.Models.Entities;
using EduSystem.Services.Helpers.Responses;
using EduSystem.Services.IServices;
using EduSystem.Utilities.Constants;
using EduSystem.Utilities.Contants;

namespace EduSystem.Services.Services;

public class UnitService : IUnitService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper  _mapper;

    public UnitService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<ResponseDto> CreateUnit(ClaimsPrincipal user, CreateUnitDto createUnitDto)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.User.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }
        
        // Lấy TeacherId từ người dùng hiện tại
        var teacher = await _unitOfWork.Teacher.GetAsync(t => t.UserId == userId);
        if (teacher == null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Teacher.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }
        
        // Map dữ liệu qua Dto và gán TeacherId từ người dùng hiện tại
        var createUnit = _mapper.Map<CreateUnitDto, Unit>(createUnitDto);
        createUnit.TeacherId = teacher.TeacherId;
        
        // Check xem UnitName đã tồn tại với gvien đó chưa
        var existingUnit = await _unitOfWork.Unit.GetAsync(u =>
                u.TeacherId == createUnit.TeacherId &&
                u.UnitName == createUnit.UnitName
        );

        if (existingUnit != null)
        {
            // Nếu tìm thấy, tức là đã tồn tại -> Trả về lỗi
            return ErrorResponse.Build(
                message: StaticResponseMessage.Unit.AlreadyExist,
                statusCode: StaticOperationStatus.StatusCode.Conflict); // 409 Conflict là mã lỗi phù hợp
        }
        
        // Cập nhật lại dữ liệu BaseEntity
        createUnit.CreatedBy = user.FindFirstValue("FullName");
        createUnit.CreatedTime = StaticOperationStatus.Timezone.Vietnam;
        createUnit.Status = StaticOperationStatus.BaseEntity.Active;
        
        try
        {
            await _unitOfWork.Unit.AddAsync(createUnit);
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Unit.NotCreated + ex.Message,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }
        
        var resultDto = _mapper.Map<UnitDto>(createUnit);
        
        return SuccessResponse.Build(
            message: StaticResponseMessage.Unit.Created,
            statusCode: StaticOperationStatus.StatusCode.Ok,
            result: resultDto);
    }

    public async Task<ResponseDto> UpdateUnit(ClaimsPrincipal user, UpdateUnitDto updateUnitDto)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.User.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        var userTeacher = await _unitOfWork.Teacher.GetAsync(t => t.UserId == userId);
        if (userTeacher is null)
        {
            return ErrorResponse.Build(
                message: "User is not a valid teacher.",
                statusCode: StaticOperationStatus.StatusCode.Forbidden); // 403 Forbidden
        }
        
        var updateUnit = await _unitOfWork.Unit.GetAsync(u => u.UnitId == updateUnitDto.UnitId,
            includeProperties: "Teacher.ApplicationUser");
        if (updateUnit is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Unit.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        // Preserve the TeacherId from the existing unit
        var updatedUnit = _mapper.Map<UpdateUnitDto, Unit>(updateUnitDto);
        updatedUnit.TeacherId = updateUnit.TeacherId; // Keep the original TeacherId
        updatedUnit.Status = StaticOperationStatus.BaseEntity.Active;
        updatedUnit.CreatedBy = updateUnit.CreatedBy;
        updatedUnit.CreatedTime = updateUnit.CreatedTime;
        updatedUnit.UpdatedBy = user.FindFirstValue("FullName");
        updatedUnit.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
        
        _unitOfWork.Unit.Update(updateUnit, updatedUnit);
        
        var resultDto =  _mapper.Map<UnitDto>(updateUnit);
        
        return (await SaveChangesAsync()) ?
            SuccessResponse.Build(
                message: StaticResponseMessage.Unit.Updated,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: resultDto)
            :
            ErrorResponse.Build(
                message: StaticResponseMessage.Unit.NotUpdated,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
    }

    public async Task<ResponseDto> GetUnitDetailsById(ClaimsPrincipal user, Guid unitId)
    {
        if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.User.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        // 1. Tải Entity Unit và Entity Teacher liên quan
        var getUnit = await _unitOfWork.Unit.GetAsync(
            u => u.UnitId == unitId && u.Status != StaticOperationStatus.BaseEntity.Deleted);
    
        if (getUnit is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Unit.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }
    
        // 2. Map Entity sang UnitDto để trả về (Bao gồm TeacherName)
        var resultUnitDto = _mapper.Map<UnitDto>(getUnit);

        return SuccessResponse.Build(
            message: StaticResponseMessage.Unit.Retrieved,
            statusCode: StaticOperationStatus.StatusCode.Ok,
            result: resultUnitDto);
    }

    public async Task<ResponseDto> GetAllUnits(
        ClaimsPrincipal user, 
        int pageNumber = 1, 
        int pageSize = 10, 
        string? filterOn = null,
        string? filterQuery = null, 
        string? sortBy = null)
    {
        try
        {
            var userRole = user.FindFirstValue(ClaimTypes.Role);
            bool isAdmin = userRole == StaticUserRoles.Admin;

            var (units, totalUnits) = await _unitOfWork.Unit.GetUnitsAsync
                (pageNumber, pageSize, filterOn, filterQuery, sortBy, isAdmin);

            if (units == null || !units.Any() || totalUnits == 0)
            {
                var emptyResult = new
                {
                    Data = Enumerable.Empty<Unit>(),
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = 0,
                    TotalPages = 0,
                    HasPreviousPage = false,
                    HasNextPage = false
                };

                return SuccessResponse.Build(
                    message: StaticResponseMessage.Unit.Found,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: emptyResult);
            }
            
            var unitsDto = _mapper.Map<IEnumerable<UnitDto>>(units);

            var result = new
            {
                Data = unitsDto,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalUnits,
                TotalPages = (int)Math.Ceiling((double)totalUnits / pageSize),
                HasPreviousPage = pageNumber > 1,
                HasNextPage = pageNumber < (int)Math.Ceiling((double)totalUnits / pageSize)
            };

            return SuccessResponse.Build(
                message: StaticResponseMessage.Unit.Retrieved,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: result);
        }
        catch (Exception ex)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Unit.NotRetrieved + ex.Message,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }
    }

    public async Task<ResponseDto> DeleteUnit(ClaimsPrincipal user, Guid unitId)
    {
        if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.User.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }
        // Include Teacher to prepare for DTO mapping
        var deleteUnit = await _unitOfWork.Unit.GetAsync(u => u.UnitId == unitId 
        &&  u.Status != StaticOperationStatus.BaseEntity.Deleted); 
        if (deleteUnit is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Unit.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        deleteUnit.Status = StaticOperationStatus.BaseEntity.Deleted;
        deleteUnit.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
        deleteUnit.UpdatedBy = user.FindFirstValue("FullName");

        if (await SaveChangesAsync())
        {
            // Map the updated entity to DTO for consistent return type
            var resultUnitDto = _mapper.Map<UnitDto>(deleteUnit); 
            return SuccessResponse.Build(
                message: StaticResponseMessage.Unit.Deleted,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: resultUnitDto); // Return DTO
        }
        else
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Unit.NotDeleted,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }
    }
    
    private async Task<bool> SaveChangesAsync()
    {
        return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
    }
}