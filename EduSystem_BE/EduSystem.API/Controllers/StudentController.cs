using EduSystem.Models.DTO.Student;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EduSystem.API.Controllers
{
    [ApiController]
    [Route("api/student")]

    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [SwaggerOperation(Summary = "API get all students's account", Description = "Requires Admin")]
        public async Task<IActionResult> GetAllStudent(
            [FromQuery] int pageNumber = 1,
            int pageSize = 10,
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null)
        {
            var response = await _studentService.GetAllStudent(
                User, pageNumber, pageSize, filterOn, filterQuery, sortBy);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("get-student-by-phone/{phoneNumber}")]
        [Authorize(Roles = "ADMIN, TEACHER")]
        [SwaggerOperation(Summary = "API get student's account by phone number", Description = "Requires Admin or Teacher")]
        public async Task<IActionResult> GetStudentByPhoneNumber([FromRoute] string phoneNumber)
        {
            var response = await _studentService.GetStudentInfoByPhoneNumber(User, phoneNumber);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{studentId:guid}")]
        [Authorize(Roles = "ADMIN")]
        [SwaggerOperation(Summary = "API gets all student's account by id", Description = "Requires Admin roles")]
        public async Task<IActionResult> GetStudentDetailsById(Guid studentId)
        {
            var response = await _studentService.GetStudentDetailsById(User, studentId);
            return StatusCode(response.StatusCode, response);
        }
        
        [HttpPut("status")]
        [Authorize(Roles = "ADMIN")]
        [SwaggerOperation(Summary = "Update student status", Description = "Requires Admin " +
        "|| Active = 1 (Đang học); Inactive = 0 (Bỏ học); Graduated = 2 (Đã tốt nghiệp); Suspended = 3 (Bị đình chỉ)")]
        public async Task<IActionResult> UpdateStudentStatus([FromBody] UpdateStudentStatusDto updateStudentStatusDto)
        {
            var response = await _studentService.UpdateStudentStatus(User, updateStudentStatusDto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
