using EduSystem.Models.DTOs.Answer;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduSystem.API.Controllers
{
    [ApiController]
    [Route("api/answer")]

    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;

        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "ADMIN,TEACHER")]
        public async Task<IActionResult> CreateAnswer([FromBody] CreateAnswerDto createDto)
        {
            var response = await _answerService.CreateAnswer(User, createDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("update")]
        [Authorize(Roles = "ADMIN,TEACHER")]
        public async Task<IActionResult> UpdateAnswer([FromBody] UpdateAnswerDto updateDto)
        {
            var response = await _answerService.UpdateAnswer(User, updateDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("get/all")]
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

        [HttpGet("get/{answerId:guid}")]
        public async Task<IActionResult> GetAnswerById([FromRoute] Guid answerId)
        {
            var response = await _answerService.GetAnswerById(User, answerId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("get/{questionId:guid}")]
        public async Task<IActionResult> GetAnswersByQuestionId([FromRoute] Guid questionId)
        {
            var response = await _answerService.GetAnswersByQuestionId(User, questionId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("delete/{answerId:guid}")]
        [Authorize(Roles = "ADMIN,TEACHER")]
        public async Task<IActionResult> DeleteAnswer([FromRoute] Guid answerId)
        {
            var response = await _answerService.DeleteAnswer(User, answerId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
