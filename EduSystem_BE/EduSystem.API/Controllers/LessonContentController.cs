using EduSystem.Models.DTOs.LessonContent;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EduSystem.API.Controllers;

[ApiController]
[Route("api/lesson-contents")]
[SwaggerTag("Lesson Content Management APIs")]

public class LessonContentController : ControllerBase
{
    private readonly ILessonContentService _lessonContentService;

    public LessonContentController(ILessonContentService lessonContentService)
    {
        _lessonContentService = lessonContentService;
    }
    
    [HttpPost()]
    [Authorize(Roles = "ADMIN,TEACHER")]
    [SwaggerOperation(Summary = "Create a new lesson content", Description = "Requires ADMIN or TEACHER role")]
    public async Task<IActionResult> CreateLessonContent([FromBody] CreateLessonContentDto createDto)
    {
        var response = await _lessonContentService.CreateLessonContent(User, createDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut()]
    [Authorize(Roles = "ADMIN,TEACHER")]
    [SwaggerOperation(Summary = "Update an existing lesson content", Description = "Requires ADMIN or TEACHER role")]
    public async Task<IActionResult> UpdateLessonContent([FromBody] UpdateLessonContentDto updateDto)
    {
        var response = await _lessonContentService.UpdateLessonContent(User, updateDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet()]
    [SwaggerOperation(Summary = "Get all lesson contents", Description = "Supports pagination, filtering, and sorting")]
    public async Task<IActionResult> GetAllLessonContents(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? filterOn = null,
        [FromQuery] string? filterQuery = null,
        [FromQuery] string? sortBy = null)
    {
        var response = await _lessonContentService.GetAllLessonContents(
            User, pageNumber, pageSize, filterOn, filterQuery, sortBy);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{contentId:guid}")]
    [SwaggerOperation(Summary = "Get lesson content by ID", Description = "Fetches a specific lesson content by its ID")]
    public async Task<IActionResult> GetLessonContentById([FromRoute] Guid contentId)
    {
        var response = await _lessonContentService.GetLessonContentById(User, contentId);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{contentId:guid}")]
    [Authorize(Roles = "ADMIN,TEACHER")]
    [SwaggerOperation(Summary = "Delete a lesson content", Description = "Requires ADMIN or TEACHER role")]
    public async Task<IActionResult> DeleteLessonContent([FromRoute] Guid contentId)
    {
        var response = await _lessonContentService.DeleteLessonContent(User, contentId);
        return StatusCode(response.StatusCode, response);
    }
}