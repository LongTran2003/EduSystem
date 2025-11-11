using EduSystem.Models.DTO;
using EduSystem.Models.DTO.Question;
using EduSystem.Models.DTO.Quiz;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EduSystem.API.Controllers;

[ApiController]
[Route("api/questions")]
[SwaggerTag("Question Management APIs")]

public class QuestionController : ControllerBase
{
    private readonly IQuestionService _questionService;
    
    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    [HttpPost]
    [Authorize(Roles = "TEACHER")]
    [SwaggerOperation(Summary = "Create a new question", Description = "Requires Teacher role")]

    public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionDto createQuestionDto)
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
        
        var response = await _questionService.CreateQuestion(User, createQuestionDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all questions", Description = "Supports pagination, filtering, and sorting")]

    public async Task<IActionResult> GetAllQuestions(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? filterOn = null,
        [FromQuery] string? filterQuery = null,
        [FromQuery] string? sortBy = null)
    {
        var response = await _questionService.GetAllQuestions(
            User, pageNumber, pageSize, filterOn, filterQuery, sortBy);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("by-teacher")]
    [Authorize(Roles = "TEACHER")]
    [SwaggerOperation(Summary = "Get questions created by current teacher", Description = "Requires Teacher role")]
    public async Task<IActionResult> GetQuestionsByCurrentTeacher(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? filterOn = null,
        [FromQuery] string? filterQuery = null,
        [FromQuery] string? sortBy = null)
    {
        var response = await _questionService.GetQuestionsByCurrentTeacher(
            User, pageNumber, pageSize, filterOn, filterQuery, sortBy);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{questionId:guid}")]
    [SwaggerOperation(Summary = "Get question by ID", Description = "Fetches a specific question by its ID")]
    public async Task<IActionResult> GetQuestionById(Guid questionId)
    {
        var response = await _questionService.GetQuestionById(User, questionId);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{questionId:guid}")]
    [Authorize(Roles = "TEACHER")]
    [SwaggerOperation(Summary = "Update an existing question", Description = "Requires Teacher role")]
    public async Task<IActionResult> UpdateQuestion([FromBody] UpdateQuestionDto updateQuestionDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _questionService.UpdateQuestion(User, updateQuestionDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{questionId:guid}")]
    [Authorize(Roles = "TEACHER")]
    [SwaggerOperation(Summary = "Delete a question (soft delete)", Description = "Requires Teacher role")]
    public async Task<IActionResult>? DeleteQuestion([FromRoute] Guid questionId)
    {
        var response = await _questionService.DeleteQuestion(User, questionId);
        return StatusCode(response.StatusCode, response);
    }
}