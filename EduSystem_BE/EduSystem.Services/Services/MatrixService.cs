using AutoMapper;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.DTO;
using EduSystem.Models.DTO.Unit;
using EduSystem.Models.DTOs.Matrix;
using EduSystem.Models.Entities;
using EduSystem.Services.Helpers.Responses;
using EduSystem.Services.IServices;
using EduSystem.Utilities.Constants;
using EduSystem.Utilities.Contants;
using System.Security.Claims;

namespace EduSystem.Services.Services
{
    public class MatrixService : IMatrixService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MatrixService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateMatrix(ClaimsPrincipal user, CreateMatrixDto createMatrixDto)
        {

            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                var teacher = await _unitOfWork.Teacher.GetAsync(t => t.UserId == userId);
                if (teacher == null)
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Teacher.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);

                // Create matrix
                var matrix = _mapper.Map<Matrix>(createMatrixDto);
                matrix.TeacherId = teacher.TeacherId;
                matrix.Status = StaticOperationStatus.BaseEntity.Active;
                matrix.CreatedBy = user.FindFirstValue("Fullname");
                matrix.CreatedTime = StaticOperationStatus.Timezone.Vietnam;

                await _unitOfWork.Matrix.AddAsync(matrix);
                await _unitOfWork.SaveAsync();

                return SuccessResponse.Build(
                    message: StaticResponseMessage.Matrix.Created,
                    statusCode: StaticOperationStatus.StatusCode.Created,
                    result: _mapper.Map<MatrixDto>(matrix));
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Matrix.NotCreated + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> UpdateMatrix(ClaimsPrincipal user, UpdateMatrixDto updateMatrixDto)
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

            var updateMatrix = await _unitOfWork.Matrix.GetAsync(u => u.MatrixId == updateMatrixDto.MatrixId,
                includeProperties: "Teacher.ApplicationUser");
            if (updateMatrix is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Matrix.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            // Preserve the TeacherId from the existing unit
            var updatedMatrix = _mapper.Map<UpdateMatrixDto, Matrix>(updateMatrixDto);
            updatedMatrix.TeacherId = updateMatrix.TeacherId; // Keep the original TeacherId
            updatedMatrix.Status = StaticOperationStatus.BaseEntity.Active;
            updatedMatrix.CreatedBy = updateMatrix.CreatedBy;
            updatedMatrix.CreatedTime = updateMatrix.CreatedTime;
            updatedMatrix.UpdatedBy = user.FindFirstValue("FullName");
            updatedMatrix.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;

            _unitOfWork.Matrix.Update(updateMatrix, updatedMatrix);

            var resultDto = _mapper.Map<MatrixDto>(updateMatrix);

            return (await SaveChangesAsync()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.Matrix.Updated,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: resultDto)
                :
                ErrorResponse.Build(
                    message: StaticResponseMessage.Matrix.NotUpdated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }

        public async Task<ResponseDto> GetMatrixById(ClaimsPrincipal user, Guid matrixId)
        {
            if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound, 
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var getMatrix = await _unitOfWork.Matrix.GetAsync(u => u.MatrixId == matrixId 
            && u.Status != StaticOperationStatus.BaseEntity.Deleted);

            if (getMatrix is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Matrix.NotFound, 
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var resultMatrixDto = _mapper.Map<MatrixDto>(getMatrix);

            return SuccessResponse.Build(
                message: StaticResponseMessage.Matrix.Retrieved, 
                statusCode: StaticOperationStatus.StatusCode.Ok, 
                result: resultMatrixDto);
        }

        public async Task<ResponseDto> GetAllMatrices(
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

                var (matrices, totalMatrices) = await _unitOfWork.Matrix.GetMatricesAsync
                    (pageNumber, pageSize, filterOn, filterQuery, sortBy, isAdmin);

                if (matrices == null || !matrices.Any() || totalMatrices == 0)
                {
                    var emptyResult = new
                    {
                        Data = Enumerable.Empty<Matrix>(),
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        TotalCount = 0,
                        TotalPages = 0,
                        HasPreviousPage = false,
                        HasNextPage = false
                    };

                    return SuccessResponse.Build(
                        message: StaticResponseMessage.Matrix.Found,
                        statusCode: StaticOperationStatus.StatusCode.Ok,
                        result: emptyResult);
                }

                var unitsDto = _mapper.Map<IEnumerable<MatrixDto>>(matrices);

                var result = new
                {
                    Data = unitsDto,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalMatrices,
                    TotalPages = (int)Math.Ceiling((double)totalMatrices / pageSize),
                    HasPreviousPage = pageNumber > 1,
                    HasNextPage = pageNumber < (int)Math.Ceiling((double)totalMatrices / pageSize)
                };

                return SuccessResponse.Build(
                    message: StaticResponseMessage.Matrix.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: result);
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Matrix.NotRetrieved + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> DeleteMatrix(ClaimsPrincipal user, Guid matrixId)
        {
            if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }
            // Include Teacher to prepare for DTO mapping
            var deleteUnit = await _unitOfWork.Matrix.GetAsync(u => u.MatrixId == matrixId
            && u.Status != StaticOperationStatus.BaseEntity.Deleted);
            if (deleteUnit is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Matrix.NotFound,
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
                    message: StaticResponseMessage.Matrix.Deleted,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: resultUnitDto); // Return DTO
            }
            else
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Matrix.NotDeleted,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        private async Task<bool> SaveChangesAsync()
        {
            return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
        }
    }
}
