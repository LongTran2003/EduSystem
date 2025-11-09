using AutoMapper;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.DTO;
using EduSystem.Models.DTOs.MatrixDetail;
using EduSystem.Models.Entities;
using EduSystem.Services.Helpers.Responses;
using EduSystem.Services.IServices;
using EduSystem.Utilities.Constants;
using EduSystem.Utilities.Contants;
using System.Security.Claims;

namespace EduSystem.Services.Services
{
    public class MatrixDetailService : IMatrixDetailService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;   

        public MatrixDetailService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto> CreateMatrixDetail(ClaimsPrincipal user, CreateMatrixDetailDto createMatrixDetailDto)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.User.UnAuthorized,
                        statusCode: StaticOperationStatus.StatusCode.Unauthorized);

                // Validate Matrix exists
                var matrix = await _unitOfWork.Matrix.GetAsync(m => m.MatrixId == createMatrixDetailDto.MatrixId);
                if (matrix == null)
                {
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Matrix.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);
                }

                var matrixDetail = _mapper.Map<MatrixDetail>(createMatrixDetailDto);
                matrixDetail.DetailId = Guid.NewGuid();
                matrixDetail.Status = StaticOperationStatus.BaseEntity.Active;
                matrixDetail.CreatedBy = user.FindFirstValue("FullName");
                matrixDetail.CreatedTime = StaticOperationStatus.Timezone.Vietnam;

                await _unitOfWork.MatrixDetail.AddAsync(matrixDetail);
                await _unitOfWork.SaveAsync();

                return SuccessResponse.Build(
                    message: StaticResponseMessage.MatrixDetail.Created,
                    statusCode: StaticOperationStatus.StatusCode.Created,
                    result: _mapper.Map<MatrixDetailDto>(matrixDetail));
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.MatrixDetail.NotCreated + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> UpdateMatrixDetail(ClaimsPrincipal user, UpdateMatrixDetailDto updateMatrixDetailDto)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.UnAuthorized,
                    statusCode: StaticOperationStatus.StatusCode.Unauthorized);
            }

            var existingDetail = await _unitOfWork.MatrixDetail.GetAsync(md =>
                md.DetailId == updateMatrixDetailDto.DetailId &&
                md.Status != StaticOperationStatus.BaseEntity.Deleted);

            if (existingDetail == null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.MatrixDetail.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            try
            {
                var updatedDetail = _mapper.Map<MatrixDetail>(existingDetail);
                _mapper.Map(updateMatrixDetailDto, updatedDetail);

                updatedDetail.UpdatedBy = user.FindFirstValue("FullName");
                updatedDetail.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;

                _unitOfWork.MatrixDetail.Update(existingDetail, updatedDetail);
                await _unitOfWork.SaveAsync();

                var resultDto = _mapper.Map<MatrixDetailDto>(updatedDetail);
                return SuccessResponse.Build(
                    message: StaticResponseMessage.MatrixDetail.Updated,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: resultDto);
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.MatrixDetail.NotUpdated + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> GetAllMatrixDetails(
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

                var (matrixDetails, totalCount) = await _unitOfWork.MatrixDetail.GetMatrixDetailsAsync(
                    pageNumber, pageSize, filterOn, filterQuery, sortBy, isAdmin, "Matrix");

                if (matrixDetails == null || !matrixDetails.Any() || totalCount == 0)
                {
                    var emptyResult = new
                    {
                        Data = Enumerable.Empty<MatrixDetailDto>(),
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        TotalCount = 0,
                        TotalPages = 0,
                        HasPreviousPage = false,
                        HasNextPage = false
                    };

                    return SuccessResponse.Build(
                        message: StaticResponseMessage.MatrixDetail.Found,
                        statusCode: StaticOperationStatus.StatusCode.Ok,
                        result: emptyResult);
                }

                var detailsDto = _mapper.Map<IEnumerable<MatrixDetailDto>>(matrixDetails);

                var result = new
                {
                    Data = detailsDto,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                    HasPreviousPage = pageNumber > 1,
                    HasNextPage = pageNumber < (int)Math.Ceiling((double)totalCount / pageSize)
                };

                return SuccessResponse.Build(
                    message: StaticResponseMessage.MatrixDetail.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: result);
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.MatrixDetail.NotRetrieved + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> GetMatrixDetailById(ClaimsPrincipal user, Guid detailId)
        {
            if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var matrixDetail = await _unitOfWork.MatrixDetail.GetAsync(
                md => md.DetailId == detailId &&
                md.Status != StaticOperationStatus.BaseEntity.Deleted,
                includeProperties: "Matrix");

            if (matrixDetail is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.MatrixDetail.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var resultDto = _mapper.Map<MatrixDetailDto>(matrixDetail);
            return SuccessResponse.Build(
                message: StaticResponseMessage.MatrixDetail.Retrieved,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: resultDto);
        }

        public async Task<ResponseDto> DeleteMatrixDetail(ClaimsPrincipal user, Guid detailId)
        {
            if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var matrixDetail = await _unitOfWork.MatrixDetail.GetAsync(md =>
                md.DetailId == detailId &&
                md.Status != StaticOperationStatus.BaseEntity.Deleted);

            if (matrixDetail is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.MatrixDetail.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            try
            {
                matrixDetail.Status = StaticOperationStatus.BaseEntity.Deleted;
                matrixDetail.UpdatedBy = user.FindFirstValue("FullName");
                matrixDetail.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;

                await _unitOfWork.SaveAsync();

                return SuccessResponse.Build(
                    message: StaticResponseMessage.MatrixDetail.Deleted,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: _mapper.Map<MatrixDetailDto>(matrixDetail));
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.MatrixDetail.NotDeleted + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }
    }
}
