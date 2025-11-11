using EduSystem.Models.DTOs.StudentAnswers;
using EduSystem.Models.DTOs.StudentAnswers.SubmitAnswer;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EduSystem.API.Controllers
{
    [ApiController]
    [Route("api/student-answers")]
    [SwaggerTag("Student Answer Management APIs")]

    public class StudentAnswerController : ControllerBase
    {
        private readonly IStudentAnswerService _studentAnswerService;

        public StudentAnswerController(IStudentAnswerService studentAnswerService)
        {
            _studentAnswerService = studentAnswerService;
        }

        [HttpPost]
        [Authorize(Roles = "STUDENT")]
        [SwaggerOperation(Summary = "Create a single student answer", Description = "Student only")]
        public async Task<IActionResult> CreateStudentAnswer([FromBody] CreateStudentAnswerDto createStudentAnswerDto)
        {
            var result = await _studentAnswerService.CreateStudentAnswer(User, createStudentAnswerDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("submit")]
        [Authorize(Roles = "STUDENT")]
        [SwaggerOperation(Summary = "Submit multiple answers at once", Description = "Batch submission for student")]
        public async Task<IActionResult> SubmitAnswers([FromBody] SubmitAnswersDto submitAnswersDto)
        {
            var result = await _studentAnswerService.SubmitAnswers(User, submitAnswersDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{attemptId:guid}/question/{questionId:guid}")]
        [SwaggerOperation(Summary = "Update a student answer", Description = "Student can update their own, Admin can update all")]
        public async Task<IActionResult> UpdateStudentAnswer([FromBody] UpdateStudentAnswerDto updateStudentAnswerDto)
        {
            var result = await _studentAnswerService.UpdateStudentAnswer(User, updateStudentAnswerDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{attemptId:guid}/question/{questionId:guid}/grade")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        [SwaggerOperation(Summary = "Grade a student answer", Description = "Teacher/Admin only")]
        public async Task<IActionResult> GradeAnswer([FromBody] UpdateStudentAnswerDto gradeDto)
        {
            var result = await _studentAnswerService.GradeAnswer(User, gradeDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{attemptId:guid}/question/{questionId:guid}")]
        [SwaggerOperation(Summary = "Get specific student answer by attempt and question ID")]
        public async Task<IActionResult> GetStudentAnswerById(
            [FromRoute] Guid attemptId,
            [FromRoute] Guid questionId)
        {
            var result = await _studentAnswerService.GetStudentAnswerById(User, attemptId, questionId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN,TEACHER")]
        [SwaggerOperation(Summary = "Get all student answers with pagination and filters", Description = "Admin/Teacher only")]
        public async Task<IActionResult> GetAllStudentAnswers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? filterOn = null,
            [FromQuery] string? filterQuery = null,
            [FromQuery] string? sortBy = null)
        {
            var result = await _studentAnswerService.GetAllStudentAnswers(
                User, pageNumber, pageSize, filterOn, filterQuery, sortBy);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("attempt/{attemptId:guid}")]
        [SwaggerOperation(Summary = "Get all answers for a specific quiz attempt")]
        public async Task<IActionResult> GetAnswersByAttemptId([FromRoute] Guid attemptId)
        {
            var result = await _studentAnswerService.GetAnswersByAttemptId(User, attemptId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{attemptId:guid}/question/{questionId:guid}")]
        [SwaggerOperation(Summary = "Delete a student answer", Description = "Student can delete their own, Admin can delete all")]
        public async Task<IActionResult> DeleteStudentAnswer(
            [FromRoute] Guid attemptId,
            [FromRoute] Guid questionId)
        {
            var result = await _studentAnswerService.DeleteStudentAnswer(User, attemptId, questionId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
