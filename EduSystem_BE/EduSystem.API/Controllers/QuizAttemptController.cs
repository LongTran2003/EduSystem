using EduSystem.Models.DTOs.QuizAttempt;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EduSystem.API.Controllers
{
    [ApiController]
    [Route("api/quiz-attempts")]
    [SwaggerTag("Quiz Attempt Management APIs")]

    public class QuizAttemptController : ControllerBase
    {

        private readonly IQuizAttemptService _quizAttemptService;

        public QuizAttemptController(IQuizAttemptService quizAttemptService)
        {
            _quizAttemptService = quizAttemptService;
        }

        [HttpPost]
        [Authorize(Roles = "STUDENT")]
        [SwaggerOperation(Summary = "Create a new quiz attempt", Description = "Requires STUDENT role")]
        public async Task<IActionResult> CreateQuizAttempt([FromBody] CreateQuizAttemptDto createQuizAttemptDto)
        {
            var result = await _quizAttemptService.CreateQuizAttempt(User, createQuizAttemptDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{quizAttemptId:guid}")]
        [SwaggerOperation(Summary = "Update an existing quiz attempt", Description = "Requires authentication")]
        public async Task<IActionResult> UpdateQuizAttempt([FromBody] UpdateQuizAttemptDto updateQuizAttemptDto)
        {
            var result = await _quizAttemptService.UpdateQuizAttempt(User, updateQuizAttemptDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{quizAttemptId:guid}")]
        [SwaggerOperation(Summary = "Get quiz attempt by ID", Description = "Fetches a specific quiz attempt by its ID")]
        public async Task<IActionResult> GetQuizAttemptById([FromRoute] Guid quizAttemptId)
        {
            var result = await _quizAttemptService.GetQuizAttemptById(User, quizAttemptId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all quiz attempts", Description = "Supports pagination, filtering, and sorting")]
        public async Task<IActionResult> GetAllQuizAttempts(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? filterOn = null,
            [FromQuery] string? filterQuery = null,
            [FromQuery] string? sortBy = null)
        {
            var result = await _quizAttemptService.GetAllQuizAttempts(
                User, pageNumber, pageSize, filterOn, filterQuery, sortBy);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-quiz/{quizId:guid}")]
        [Authorize(Roles = "ADMIN,TEACHER")]
        [SwaggerOperation(Summary = "Get quiz attempts by quiz ID", Description = "Requires ADMIN or TEACHER role")]
        public async Task<IActionResult> GetQuizAttemptsByQuizId(
            [FromRoute] Guid quizId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _quizAttemptService.GetQuizAttemptsByQuizId(User, quizId, pageNumber, pageSize);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-student/{studentId:guid}")]
        [SwaggerOperation(Summary = "Get quiz attempts by student ID", Description = "Fetches all attempts of a specific student")]
        public async Task<IActionResult> GetQuizAttemptsByStudentId(
            [FromRoute] Guid studentId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _quizAttemptService.GetQuizAttemptsByStudentId(User, studentId, pageNumber, pageSize);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{quizAttemptId:guid}")]
        [SwaggerOperation(Summary = "Delete a quiz attempt", Description = "Requires authentication")]
        public async Task<IActionResult> DeleteQuizAttempt([FromRoute] Guid quizAttemptId)
        {
            var result = await _quizAttemptService.DeleteQuizAttempt(User, quizAttemptId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
