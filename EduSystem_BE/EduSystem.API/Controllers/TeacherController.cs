using EduSystem.Models.DTOs.Teacher;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EduSystem.API.Controllers
{
    [ApiController]
    [Route("api/teacher")]

    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        //[Authorize(Roles = "ADMIN")]
        [SwaggerOperation(Summary = "API get all teachers's account", Description = "Requires Admin")]
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

        [HttpGet("get-student-by-phone/{phoneNumber}")]
        [Authorize(Roles = "ADMIN, TEACHER")]
        [SwaggerOperation(Summary = "API get teacher's account by phone number", Description = "Requires Admin or Teacher")]
        public async Task<IActionResult> GetStudentByPhoneNumber([FromRoute] string phoneNumber)
        {
            var response = await _teacherService.GetTeacherInfoByPhoneNumber(User, phoneNumber);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{teacherId:guid}")]
        [Authorize(Roles = "ADMIN")]
        [SwaggerOperation(Summary = "API gets all teacher's account by id", Description = "Requires admin roles")]
        public async Task<IActionResult> GetStudentDetailsById(Guid studentId)
        {
            var response = await _teacherService.GetTeacherDetailsById(User, studentId);
            return StatusCode(response.StatusCode, response);
        }
        
        [HttpPut("status")]
        [Authorize(Roles = "ADMIN")]
        [SwaggerOperation(Summary = "Update teacher status", Description = "Requires Admin role " +
        "|| Active = 1 (Hoạt động); Inactive = 0 (Không hoạt động); OnLeave = 2 (Nghỉ phép); Retired = 3 (Đã nghỉ hưu)")]
        public async Task<IActionResult> UpdateTeacherStatus([FromBody] UpdateTeacherStatusDto updateTeacherStatusDto)
        {
            var response = await _teacherService.UpdateTeacherStatus(User, updateTeacherStatusDto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
