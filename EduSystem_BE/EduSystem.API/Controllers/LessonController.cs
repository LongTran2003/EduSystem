using EduSystem.Models.DTO;
using EduSystem.Models.DTO.Lesson;
using EduSystem.Models.DTO.Subject;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EduSystem.API.Controllers;

[ApiController]
[Route("api/lesson")]

public class LessonController : ControllerBase
{
    private readonly ILessonService _lessonService;
    
    public LessonController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }
    
    [HttpPost("create")]
    [Authorize(Roles = "TEACHER")]
    [SwaggerOperation(Summary = "Creates a new lesson", Description = "Requires Teacher role")]
    public async Task<IActionResult> CreateLesson([FromBody] CreateLessonDto createLessonDto)
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
        
        var response = await _lessonService.CreateLesson(User, createLessonDto);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpGet]
    [SwaggerOperation(Summary = "Get all lessons", Description = "Requires authentication")]
    public async Task<IActionResult> GetAllLessons(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? filterOn = null,
        [FromQuery] string? filterQuery = null,
        [FromQuery] string? sortBy = null)
    {
        var response = await _lessonService.GetAllLessons(
            User, pageNumber, pageSize, filterOn, filterQuery, sortBy);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut]
    [Authorize(Roles = "TEACHER")]
    [SwaggerOperation(Summary = "Update lesson", Description = "Requires Teacher role")]
    public async Task<IActionResult> UpdateLesson([FromBody] UpdateLessonDto updateLessonDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _lessonService.UpdateLesson(User, updateLessonDto);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpDelete("{lessonId:guid}")]
    [Authorize(Roles = "TEACHER")]
    [SwaggerOperation(Summary = "Delete lesson (soft delete)", Description = "Requires Teacher role")]
    public async Task<IActionResult>? DeleteLesson([FromRoute] Guid lessonId)
    {
        var response = await _lessonService.DeleteLesson(User, lessonId);
        return StatusCode(response.StatusCode, response);
    }
    
}