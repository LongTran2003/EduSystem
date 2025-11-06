using EduSystem.Models.DTOs.LessonContent;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduSystem.API.Controllers;

[ApiController]
[Route("api/lessoncontent")]

public class LessonContentController : ControllerBase
{
    private readonly ILessonContentService _lessonContentService;

    public LessonContentController(ILessonContentService lessonContentService)
    {
        _lessonContentService = lessonContentService;
    }
    
    [HttpPost("create")]
    [Authorize(Roles = "ADMIN,TEACHER")]
    public async Task<IActionResult> CreateLessonContent([FromBody] CreateLessonContentDto createDto)
    {
        var response = await _lessonContentService.CreateLessonContent(User, createDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("update")]
    [Authorize(Roles = "ADMIN,TEACHER")]
    public async Task<IActionResult> UpdateLessonContent([FromBody] UpdateLessonContentDto updateDto)
    {
        var response = await _lessonContentService.UpdateLessonContent(User, updateDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get/all")]
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

    [HttpGet("get/{contentId:guid}")]
    public async Task<IActionResult> GetLessonContentById([FromRoute] Guid contentId)
    {
        var response = await _lessonContentService.GetLessonContentById(User, contentId);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete/{contentId:guid}")]
    [Authorize(Roles = "ADMIN,TEACHER")]
    public async Task<IActionResult> DeleteLessonContent([FromRoute] Guid contentId)
    {
        var response = await _lessonContentService.DeleteLessonContent(User, contentId);
        return StatusCode(response.StatusCode, response);
    }
}