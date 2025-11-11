using EduSystem.Models.DTO;
using EduSystem.Models.DTO.Quiz;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EduSystem.API.Controllers;

[ApiController]
[Route("api/quizzes")]
[SwaggerTag("Quiz Management APIs")]

public class QuizController : ControllerBase
{
    private readonly IQuizService _quizService;
    
    public QuizController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    [HttpPost]
    [Authorize(Roles = "TEACHER")]
    [SwaggerOperation(Summary = "Create a new quiz", Description = "Requires Teacher role")]
    public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizDto createQuizDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResponseDto
            {
                IsSuccess = false,
                Message = "Invalid input data.",
                Result = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
            });
        }
        
        var response = await _quizService.CreateQuiz(User, createQuizDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all quizzes", Description = "Supports pagination, filtering, and sorting")]
    public async Task<IActionResult> GetAllQuizzes(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? filterOn = null,
        [FromQuery] string? filterQuery = null,
        [FromQuery] string? sortBy = null)
    {
        var response = await _quizService.GetAllQuizzes(
            User, pageNumber, pageSize, filterOn, filterQuery, sortBy);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{quizId:guid}")]
    [SwaggerOperation(Summary = "Get quiz detail by ID", Description = "Fetches a specific quiz by its ID")]
    public async Task<IActionResult> GetQuizDetail(Guid quizId)
    {
        var response = await _quizService.GetQuizById(User, quizId);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{quizId:guid}")]
    [Authorize(Roles = "TEACHER")]
    [SwaggerOperation(Summary = "Update an existing quiz", Description = "Requires Teacher role")]
    public async Task<IActionResult> UpdateQuiz([FromBody] UpdateQuizDto updateQuizDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _quizService.UpdateQuiz(User, updateQuizDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{quizId:guid}")]
    [Authorize(Roles = "TEACHER")]
    [SwaggerOperation(Summary = "Delete a quiz (soft delete)", Description = "Requires Teacher role")]
    public async Task<IActionResult>? DeleteQuiz([FromRoute] Guid quizId)
    {
        var response = await _quizService.DeleteQuiz(User, quizId);
        return StatusCode(response.StatusCode, response);
    }
}