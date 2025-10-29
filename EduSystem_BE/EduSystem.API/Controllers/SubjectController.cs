using EduSystem.Models.DTO;
using EduSystem.Models.DTO.Subject;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EduSystem.API.Controllers;

[ApiController]
[Route("api/subject")]

public class SubjectController : ControllerBase
{
    private readonly ISubjectService _subjectService;
    
    public SubjectController(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    [HttpPost("create")]
    [Authorize(Roles = "TEACHER")]
    [SwaggerOperation(Summary = "Creates a new subject", Description = "Requires Teacher role")]
    public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectDto createSubjectDto)
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
        
        var response = await _subjectService.CreateSubject(User, createSubjectDto);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpGet]
    [SwaggerOperation(Summary = "Get all subjects", Description = "Requires authentication")]
    public async Task<IActionResult> GetAllSubjects(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? filterOn = null,
        [FromQuery] string? filterQuery = null,
        [FromQuery] string? sortBy = null)
    {
        var response = await _subjectService.GetAllSubjects(
            User, pageNumber, pageSize, filterOn, filterQuery, sortBy);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut]
    [Authorize(Roles = "TEACHER")]
    [SwaggerOperation(Summary = "Update subject", Description = "Requires Teacher role")]
    public async Task<IActionResult> UpdateSubject([FromBody] UpdateSubjectDto updateSubjectDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _subjectService.UpdateSubject(User, updateSubjectDto);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpDelete("{subjectId:guid}")]
    [Authorize(Roles = "TEACHER")]
    [SwaggerOperation(Summary = "Delete subject (soft delete)", Description = "Requires Admin role")]
    public async Task<IActionResult>? DeleteSubject([FromRoute] Guid subjectId)
    {
        var response = await _subjectService.DeleteSubject(User, subjectId);
        return StatusCode(response.StatusCode, response);
    }
}