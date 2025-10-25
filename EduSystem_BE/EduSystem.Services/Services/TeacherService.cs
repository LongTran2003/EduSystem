using AutoMapper;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.DTO;
using EduSystem.Models.DTOs.Teacher;
using EduSystem.Models.Entities;
using EduSystem.Services.Helpers.Responses;
using EduSystem.Services.IServices;
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

        public async Task<ResponseDto> GetAllTeachers()
        {
            var teacherFromDB = await _unitOfWork.Teacher.GetAllAsync(includeProperties: nameof(ApplicationUser));

            var teachersDto = _mapper.Map<IEnumerable<Models.Entities.Teacher>>(teacherFromDB);

            return teacherFromDB.Any()
                ? SuccessResponse.Build(
                    message: StaticResponseMessage.Teacher.Found,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: teachersDto)
                : ErrorResponse.Build(
                    message: StaticResponseMessage.Teacher.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
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
    }
}
