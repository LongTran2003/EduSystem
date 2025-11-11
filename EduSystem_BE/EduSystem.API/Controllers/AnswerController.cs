using EduSystem.Models.DTOs.Answer;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EduSystem.API.Controllers
{
    [ApiController]
    [Route("api/answer")]
    [SwaggerTag("Answer Management APIs")]

    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;

        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpPost()]
        [Authorize(Roles = "ADMIN,TEACHER")]
        [SwaggerOperation(Summary = "Create a new answer", Description = "Requires ADMIN or TEACHER role")]
        public async Task<IActionResult> CreateAnswer([FromBody] CreateAnswerDto createDto)
        {
            var response = await _answerService.CreateAnswer(User, createDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{answerId:guid}")]
        [Authorize(Roles = "ADMIN,TEACHER")]
        [SwaggerOperation(Summary = "Update an existing answer", Description = "Requires ADMIN or TEACHER role")]
        public async Task<IActionResult> UpdateAnswer([FromBody] UpdateAnswerDto updateDto)
        {
            var response = await _answerService.UpdateAnswer(User, updateDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet()]
        [SwaggerOperation(Summary = "Get all answers", Description = "Supports pagination, filtering, and sorting")]
        public async Task<IActionResult> GetAllAnswers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? filterOn = null,
            [FromQuery] string? filterQuery = null,
            [FromQuery] string? sortBy = null)
        {
            var response = await _answerService.GetAllAnswers(
                User, pageNumber, pageSize, filterOn, filterQuery, sortBy);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{answerId:guid}")]
        [SwaggerOperation(Summary = "Get answer by ID", Description = "Fetches a specific answer by its ID")]
        public async Task<IActionResult> GetAnswerById([FromRoute] Guid answerId)
        {
            var response = await _answerService.GetAnswerById(User, answerId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("by-question/{questionId:guid}")]
        [SwaggerOperation(Summary = "Get answers by question ID", Description = "Fetches all answers linked to a specific question")]
        public async Task<IActionResult> GetAnswersByQuestionId([FromRoute] Guid questionId)
        {
            var response = await _answerService.GetAnswersByQuestionId(User, questionId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{answerId:guid}")]
        [Authorize(Roles = "ADMIN,TEACHER")]
        [SwaggerOperation(Summary = "Delete an answer", Description = "Requires ADMIN or TEACHER role")]
        public async Task<IActionResult> DeleteAnswer([FromRoute] Guid answerId)
        {
            var response = await _answerService.DeleteAnswer(User, answerId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
