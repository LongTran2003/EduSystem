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
        public async Task<IActionResult> GetAllStudent()
        {
            var response = await _studentService.GetAllStudent();
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
        [SwaggerOperation(Summary = "API gets all student's account by id", Description = "Requires admin roles")]
        public async Task<IActionResult> GetStudentDetailsById(Guid studentId)
        {
            var response = await _studentService.GetStudentDetailsById(User, studentId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
