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
        if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.User.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }
        
        // Map dữ liệu qua Dto
        var createUnit = _mapper.Map<CreateUnitDto, Unit>(createUnitDto);
        
        // Check xem UnitName đã tồn tại với gvien đó chưa
        var existingUnit = await _unitOfWork.Unit.GetAsync(u =>
                u.TeacherId == createUnit.TeacherId &&
                u.UnitName == createUnit.UnitName
        );

        if (existingUnit != null)
        {
            // Nếu tìm thấy, tức là đã tồn tại -> Trả về lỗi
            // Bạn nên tạo một message mới trong StaticResponseMessage, ví dụ: Unit.AlreadyExists
            return ErrorResponse.Build(
                message: StaticResponseMessage.Unit.AlreadyExist,
                statusCode: StaticOperationStatus.StatusCode.Conflict); // 409 Conflict là mã lỗi phù hợp
        }
        
        // Cập nhật lại dữ liệu BaseEntity
        createUnit.CreatedBy = user.FindFirstValue("FullName");
        createUnit.CreatedTime = StaticOperationStatus.Timezone.Vietnam;
        createUnit.Status = StaticOperationStatus.BaseEntity.Active;
        
        // Map trả lại kết quả qua UnitDto
        createUnit.Teacher = await _unitOfWork.Teacher.GetAsync(u => u.TeacherId ==  createUnit.TeacherId, 
            includeProperties: "ApplicationUser");
        
        try
        {
            await _unitOfWork.Unit.AddAsync(createUnit);
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Unit.NotCreated,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }
        
        var result = _mapper.Map<UnitDto>(createUnit);
        
        return SuccessResponse.Build(
            message: StaticResponseMessage.Unit.Created,
            statusCode: StaticOperationStatus.StatusCode.Ok,
            result: result);
    }

    public async Task<ResponseDto> UpdateUnit(ClaimsPrincipal user, UpdateUnitDto updateUnitDto)
    {
        if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.User.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        var updateUnit = await _unitOfWork.Unit.GetAsync(u => u.UnitId == updateUnitDto.UnitId);
        if (updateUnit is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Unit.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        var updatedUnit = _mapper.Map<UpdateUnitDto, Unit>(updateUnitDto);
        _unitOfWork.Unit.Update(updateUnit, updatedUnit);

        return (!await SaveChangesAsync()) ?
            ErrorResponse.Build(
                message: StaticResponseMessage.Unit.NotUpdated,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError)
            :
            SuccessResponse.Build(
                message: StaticResponseMessage.Unit.Updated,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: updatedUnit);
    }

    public async Task<ResponseDto> GetUnitDetailsById(ClaimsPrincipal user, Guid unitId)
    {
        if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.User.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        var getUnit = await _unitOfWork.Unit.GetAsync(u => u.UnitId == unitId && u.Status != StaticOperationStatus.BaseEntity.Deleted);
        return (getUnit is null) ?
            ErrorResponse.Build(
                message: StaticResponseMessage.Unit.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound)
            :
            SuccessResponse.Build(
                message: StaticResponseMessage.Unit.Retrieved,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: getUnit);
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
        var deleteUnit = await _unitOfWork.Unit.GetAsync(u => u.UnitId == unitId);
        if (deleteUnit is null)
        {
            return ErrorResponse.Build(
                message: StaticResponseMessage.Unit.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        deleteUnit.Status = StaticOperationStatus.BaseEntity.Deleted;
        deleteUnit.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
        deleteUnit.UpdatedBy = user.FindFirstValue("FullName");

        return (await SaveChangesAsync()) ?
            SuccessResponse.Build(
                message: StaticResponseMessage.Unit.Deleted,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: deleteUnit)
            :
            ErrorResponse.Build(
                message: StaticResponseMessage.Unit.NotDeleted,
                statusCode: StaticOperationStatus.StatusCode.InternalServerError);
    }
    
    private async Task<bool> SaveChangesAsync()
    {
        return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
    }
}