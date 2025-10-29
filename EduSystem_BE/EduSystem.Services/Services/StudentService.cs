using AutoMapper;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.DTO;
using EduSystem.Models.DTO.Student;
using EduSystem.Models.Entities;
using EduSystem.Services.Helpers.Responses;
using EduSystem.Services.IServices;
using EduSystem.Utilities.Constants;
using EduSystem.Utilities.Contants;
using System.Security.Claims;

namespace EduSystem.Services.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> GetAllStudent
            (
            ClaimsPrincipal User,
            int pageNumber = 1,
            int pageSize = 10,
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null
            )
        {
            try
            {
                var userRole = User.FindFirstValue(ClaimTypes.Role);
                bool isAdmin = userRole == StaticUserRoles.Admin;

                var (students, totalStudents) = await _unitOfWork.Student
                    .GetStudentsAsync(
                        pageNumber,
                        pageSize,
                        filterOn,
                        filterQuery,
                        sortBy,
                        isAdmin,
                        includeProperties: nameof(ApplicationUser)
                    );

                if (students == null || !students.Any() || totalStudents == 0)
                {
                    var emptyResult = new
                    {
                        Data = Enumerable.Empty<Student>(),
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        TotalCount = 0,
                        TotalPages = 0,
                        HasPreviousPage = false,
                        HasNextPage = false
                    };

                    return SuccessResponse.Build(
                        message: StaticResponseMessage.Student.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.Ok,
                        result: emptyResult);
                }

                var studentsDto = _mapper.Map<IEnumerable<Student>>(students);

                var result = new
                {
                    Data = studentsDto,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalStudents,
                    TotalPages = (int)Math.Ceiling((double)totalStudents / pageSize),
                    HasPreviousPage = pageNumber > 1,
                    HasNextPage = pageNumber < (int)Math.Ceiling((double)totalStudents / pageSize)
                };

                return SuccessResponse.Build(
                    message: StaticResponseMessage.Student.Found,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: result);
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Student.NotRetrieved + ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError
                );
            }


            //var studentFromDB = await _unitOfWork.Student.GetAllAsync(includeProperties: nameof(ApplicationUser));

            //var studentsDto = _mapper.Map<IEnumerable<Student>>(studentFromDB);

            //return studentFromDB.Any()
            //    ? SuccessResponse.Build(
            //        message: StaticResponseMessage.Student.Found,
            //        statusCode: StaticOperationStatus.StatusCode.Ok,
            //        result: studentsDto)
            //    : ErrorResponse.Build(
            //        message: StaticResponseMessage.Student.NotFound,
            //        statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        public async Task<ResponseDto> GetStudentDetailsById(ClaimsPrincipal user, Guid studentId)
        {
            if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }
            var studentFromDb = await _unitOfWork.Student.GetAsync(
                filter: c => c.StudentId == studentId,
                includeProperties: nameof(ApplicationUser));
            if (studentFromDb is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Student.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }
            var customerDto = _mapper.Map<GetStudentDto>(studentFromDb);

            return SuccessResponse.Build(
                message: StaticResponseMessage.Student.Found,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: customerDto);
        }

        public async Task<ResponseDto> GetStudentInfoByPhoneNumber(ClaimsPrincipal user, string phoneNumber)
        {
            if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound
                );
            }

            var student = (await _unitOfWork.Student.GetAsync(
                filter: x => x.ApplicationUser.PhoneNumber == phoneNumber,
                includeProperties: nameof(ApplicationUser)));

            if (student == null)
            {
                return new ResponseDto
                {
                    Result = null,
                    IsSuccess = false,
                    StatusCode = StaticOperationStatus.StatusCode.BadRequest,
                    Message = StaticResponseMessage.Student.NotExisted
                };
            }

            return new ResponseDto
            {
                Result = student,
                IsSuccess = true,
                StatusCode = StaticOperationStatus.StatusCode.Ok,
                Message = StaticResponseMessage.Student.Found
            };
        }
    }
}
