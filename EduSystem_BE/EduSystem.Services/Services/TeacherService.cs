using AutoMapper;
using EduSystem.Repository.IRepositories;
using EduSystem.Models.DTO;
using EduSystem.Models.DTOs.Teacher;
using EduSystem.Models.Entities;
using EduSystem.Services.Helpers.Responses;
using EduSystem.Services.IServices;
using EduSystem.Utilities.Constants;
using EduSystem.Utilities.Contants;
using System.Security.Claims;
using static EduSystem.Utilities.Contants.StaticResponseMessage;

namespace EduSystem.Services.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeacherService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> GetAllTeachers
            (
            ClaimsPrincipal user,
            int pageNumber = 1,
            int pageSize = 10,
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null
            )
        {
            try
            {
                var userRole = user.FindFirstValue(ClaimTypes.Role);
                bool isAdmin = userRole == StaticUserRoles.Admin;

                var (teachers, totalTeachers) = await _unitOfWork.Teacher
                    .GetTeachersAsync(
                        pageNumber,
                        pageSize,
                        filterOn,
                        filterQuery,
                        sortBy,
                        isAdmin,
                        includeProperties: nameof(ApplicationUser)
                    );
                
                if (teachers == null || !teachers.Any() || totalTeachers == 0)
                {
                    var emptyResult = new
                    {
                        Data = Enumerable.Empty<Models.Entities.Teacher>(),
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        TotalCount = 0,
                        TotalPages = 0,
                        HasPreviousPage = false,
                        HasNextPage = false
                    };

                    return SuccessResponse.Build(
                        message: StaticResponseMessage.Teacher.Found,
                        statusCode: StaticOperationStatus.StatusCode.Ok,
                        result: emptyResult);
                }
                
                var teachersDto = _mapper.Map<IEnumerable<GetTeacherDto>>(teachers);

                var result = new
                {
                    Data = teachersDto,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalTeachers,
                    TotalPages = (int)Math.Ceiling((double)totalTeachers / pageSize),
                    HasPreviousPage = pageNumber > 1,
                    HasNextPage = pageNumber < (int)Math.Ceiling((double)totalTeachers / pageSize)
                };

                return SuccessResponse.Build(
                    message: StaticResponseMessage.Teacher.Found,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: result
                );
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: $"An error occurred while retrieving teachers: {ex.Message}",
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError
                );
            }

            //var teacherFromDB = await _unitOfWork.Teacher.GetAllAsync(includeProperties: nameof(ApplicationUser));

            //var teachersDto = _mapper.Map<IEnumerable<Models.Entities.Teacher>>(teacherFromDB);

            //return teacherFromDB.Any()
            //    ? SuccessResponse.Build(
            //        message: StaticResponseMessage.Teacher.Found,
            //        statusCode: StaticOperationStatus.StatusCode.Ok,
            //        result: teachersDto)
            //    : ErrorResponse.Build(
            //        message: StaticResponseMessage.Teacher.NotFound,
            //        statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        public async Task<ResponseDto> GetTeacherDetailsById(ClaimsPrincipal user, Guid teacherId)
        {
            if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }
            var teacherFromDb = await _unitOfWork.Teacher.GetAsync(
                filter: c => c.TeacherId == teacherId,
                includeProperties: nameof(ApplicationUser));
            if (teacherFromDb is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Teacher.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }
            var teacherDto = _mapper.Map<GetTeacherDto>(teacherFromDb);

            return SuccessResponse.Build(
                message: StaticResponseMessage.Teacher.Found,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: teacherDto);
        }

        public async Task<ResponseDto> GetTeacherInfoByPhoneNumber(ClaimsPrincipal user, string phoneNumber)
        {
            if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound
                );
            }

            var teacher = (await _unitOfWork.Teacher.GetAsync(
                filter: x => x.ApplicationUser.PhoneNumber == phoneNumber,
                includeProperties: nameof(ApplicationUser)));

            if (teacher == null)
            {
                return new ResponseDto
                {
                    Result = null,
                    IsSuccess = false,
                    StatusCode = StaticOperationStatus.StatusCode.BadRequest,
                    Message = StaticResponseMessage.Teacher.NotExisted
                };
            }

            return new ResponseDto
            {
                Result = teacher,
                IsSuccess = true,
                StatusCode = StaticOperationStatus.StatusCode.Ok,
                Message = StaticResponseMessage.Teacher.Found
            };
        }

        public async Task<ResponseDto> UpdateTeacherStatus(ClaimsPrincipal user, UpdateTeacherStatusDto updateTeacherStatusDto)
        {
            if (user.FindFirstValue(ClaimTypes.Role) != StaticUserRoles.Admin)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotAuthorized,
                    statusCode: StaticOperationStatus.StatusCode.Forbidden);
            }

            var teacher = await _unitOfWork.Teacher.GetAsync(t => t.TeacherId == updateTeacherStatusDto.TeacherId);
            if (teacher is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Teacher.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            teacher.Status = updateTeacherStatusDto.Status;

            var saved = await _unitOfWork.SaveAsync();
            return (saved == StaticOperationStatus.Database.Success)
                ? SuccessResponse.Build(
                    message: StaticResponseMessage.Teacher.Updated,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: new { teacher.TeacherId, teacher.Status })
                : ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotUpdated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }
    }
}
