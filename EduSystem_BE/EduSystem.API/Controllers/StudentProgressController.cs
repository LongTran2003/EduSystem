using EduSystem.Models.DTO;
using EduSystem.Models.DTOs.StudentProgresses;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EduSystem.API.Controllers
{
    [ApiController]
    [Route("api/student-progresses")]
    [SwaggerTag("Student Progress Management APIs")]

    public class StudentProgressController : ControllerBase
    {
        private readonly IStudentProgressService _studentProgressService;

        public StudentProgressController(IStudentProgressService studentProgressService)
        {
            _studentProgressService = studentProgressService;
        }

        [HttpPost]
        [Authorize(Roles = "TEACHER,ADMIN")]
        [SwaggerOperation(Summary = "Create a new student progress record", Description = "Requires Teacher or Admin role")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDto>> CreateStudentProgress([FromBody] CreateStudentProgressDto createStudentProgressDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _studentProgressService.CreateStudentProgress(User, createStudentProgressDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{progressId:guid}")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        [SwaggerOperation(Summary = "Update student progress by ID", Description = "Requires Teacher or Admin role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDto>> UpdateStudentProgress([FromBody] UpdateStudentProgressDto updateStudentProgressDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _studentProgressService.UpdateStudentProgress(User, updateStudentProgressDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{progressId:guid}/complete-lesson")]
        [Authorize(Roles = "STUDENT")]
        [SwaggerOperation(Summary = "Mark lesson as completed", Description = "Student only")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDto>> CompleteLessonProgress([FromBody] UpdateProgressDto updateProgressDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _studentProgressService.CompleteLessonProgress(User, updateProgressDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{progressId:guid}")]
        [SwaggerOperation(Summary = "Get student progress by ID", Description = "Requires authentication")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDto>> GetStudentProgressById(Guid progressId)
        {
            var response = await _studentProgressService.GetStudentProgressById(User, progressId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("student/{studentId:guid}")]
        [SwaggerOperation(Summary = "Get all progress records for a student", Description = "Requires Teacher or Admin role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseDto>> GetProgressByStudentId(Guid studentId)
        {
            var response = await _studentProgressService.GetProgressByStudentId(User, studentId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("unit/{unitId:guid}")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        [SwaggerOperation(Summary = "Get all progress records for a unit", Description = "Requires Teacher or Admin role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseDto>> GetProgressByUnitId(Guid unitId)
        {
            var response = await _studentProgressService.GetProgressByUnitId(User, unitId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Authorize(Roles = "TEACHER,ADMIN")]
        [SwaggerOperation(Summary = "Get all student progress records with pagination and filtering", Description = "Requires Teacher or Admin role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseDto>> GetAllStudentProgress(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? filterOn = null,
            [FromQuery] string? filterQuery = null,
            [FromQuery] string? sortBy = null)
        {
            var response = await _studentProgressService.GetAllStudentProgress(User, pageNumber, pageSize, filterOn, filterQuery, sortBy);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{progressId:guid}")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        [SwaggerOperation(Summary = "Delete student progress by ID", Description = "Requires Teacher or Admin role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDto>> DeleteStudentProgress(Guid progressId)
        {
            var response = await _studentProgressService.DeleteStudentProgress(User, progressId);
            return StatusCode(response.StatusCode, response);
        }

    }
}
