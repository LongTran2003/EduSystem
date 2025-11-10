using EduSystem.Models.DTOs.QuizAttempt;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduSystem.API.Controllers
{
    [ApiController]
    [Route("api/quiz-attempt")]

    public class QuizAttemptController : ControllerBase
    {

        private readonly IQuizAttemptService _quizAttemptService;

        public QuizAttemptController(IQuizAttemptService quizAttemptService)
        {
            _quizAttemptService = quizAttemptService;
        }

        [HttpPost]
        [Authorize(Roles = "STUDENT")]
        public async Task<IActionResult> CreateQuizAttempt([FromBody] CreateQuizAttemptDto createQuizAttemptDto)
        {
            var result = await _quizAttemptService.CreateQuizAttempt(User, createQuizAttemptDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuizAttempt([FromBody] UpdateQuizAttemptDto updateQuizAttemptDto)
        {
            var result = await _quizAttemptService.UpdateQuizAttempt(User, updateQuizAttemptDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetQuizAttemptById([FromRoute] Guid id)
        {
            var result = await _quizAttemptService.GetQuizAttemptById(User, id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
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

        [HttpGet("quiz/{quizId:guid}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetQuizAttemptsByQuizId(
            [FromRoute] Guid quizId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _quizAttemptService.GetQuizAttemptsByQuizId(User, quizId, pageNumber, pageSize);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("student/{studentId:guid}")]
        public async Task<IActionResult> GetQuizAttemptsByStudentId(
            [FromRoute] Guid studentId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _quizAttemptService.GetQuizAttemptsByStudentId(User, studentId, pageNumber, pageSize);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteQuizAttempt([FromRoute] Guid id)
        {
            var result = await _quizAttemptService.DeleteQuizAttempt(User, id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
