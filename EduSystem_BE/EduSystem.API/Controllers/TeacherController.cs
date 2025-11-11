using EduSystem.Models.DTOs.Teacher;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EduSystem.API.Controllers
{
    [ApiController]
    [Route("api/teachers")]
    [SwaggerTag("Teacher Management APIs")]

    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [SwaggerOperation(Summary = "Get all teachers' accounts", Description = "Requires Admin role")]
        public async Task<IActionResult> GetAllTeachers(
            [FromQuery] int pageNumber = 1,
            int pageSize = 10,
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null)
        {
            var response = await _teacherService.GetAllTeachers(
                User, pageNumber, pageSize, filterOn, filterQuery, sortBy);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("get-by-phone/{phoneNumber}")]
        [Authorize(Roles = "ADMIN,TEACHER")]
        [SwaggerOperation(Summary = "Get teacher account by phone number", Description = "Requires Admin or Teacher role")]
        public async Task<IActionResult> GetStudentByPhoneNumber([FromRoute] string phoneNumber)
        {
            var response = await _teacherService.GetTeacherInfoByPhoneNumber(User, phoneNumber);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{teacherId:guid}")]
        [Authorize(Roles = "ADMIN")]
        [SwaggerOperation(Summary = "Get teacher details by ID", Description = "Requires Admin role")]
        public async Task<IActionResult> GetTeacherDetailsById([FromRoute] Guid teacherId)
        {
            var response = await _teacherService.GetTeacherDetailsById(User, teacherId);
            return StatusCode(response.StatusCode, response);
        }


        [HttpPut("{teacherId:guid}/status")]
        [Authorize(Roles = "ADMIN")]
        [SwaggerOperation(Summary = "Update teacher status by ID", Description = "Requires Admin role || Active = 1; Inactive = 0; OnLeave = 2; Retired = 3")]
        public async Task<IActionResult> UpdateTeacherStatus([FromBody] UpdateTeacherStatusDto updateTeacherStatusDto)
        {
            var response = await _teacherService.UpdateTeacherStatus(User, updateTeacherStatusDto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
