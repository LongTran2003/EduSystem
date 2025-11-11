using EduSystem.Models.DTO.Student;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EduSystem.API.Controllers
{
    [ApiController]
    [Route("api/students")]
    [SwaggerTag("Student Management APIs")]

    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [SwaggerOperation(Summary = "Get all students", Description = "Requires Admin role; supports pagination, filtering, sorting")]
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

        [HttpGet("by-phone/{phoneNumber}")]
        [Authorize(Roles = "ADMIN,TEACHER")]
        [SwaggerOperation(Summary = "Get student by phone number", Description = "Requires Admin or Teacher role")]
        public async Task<IActionResult> GetStudentByPhoneNumber([FromRoute] string phoneNumber)
        {
            var response = await _studentService.GetStudentInfoByPhoneNumber(User, phoneNumber);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{studentId:guid}")]
        [Authorize(Roles = "ADMIN")]
        [SwaggerOperation(Summary = "Get student details by ID", Description = "Requires Admin role")]
        public async Task<IActionResult> GetStudentDetailsById(Guid studentId)
        {
            var response = await _studentService.GetStudentDetailsById(User, studentId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{studentId:guid}/status")]
        [Authorize(Roles = "ADMIN")]
        [SwaggerOperation(Summary = "Update student status",
            Description = "Requires Admin role. Active=1, Inactive=0, Graduated=2, Suspended=3")]
        public async Task<IActionResult> UpdateStudentStatus([FromBody] UpdateStudentStatusDto updateStudentStatusDto)
        {
            var response = await _studentService.UpdateStudentStatus(User, updateStudentStatusDto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
