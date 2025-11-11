using EduSystem.Models.DTO;
using EduSystem.Models.DTO.Unit;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EduSystem.API.Controllers;

[ApiController]
[Route("api/units")]
[SwaggerTag("Unit Management APIs")]

public class UnitController : ControllerBase
{
    private readonly IUnitService _unitService;
    
    public UnitController(IUnitService unitService)
    {
        _unitService = unitService;
    }

    [HttpPost]
    [Authorize(Roles = "TEACHER")]
    [SwaggerOperation(Summary = "Create a new unit", Description = "Requires Teacher role")]
    public async Task<IActionResult> CreateUnit([FromBody] CreateUnitDto createUnitDto)
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
        
        var response = await _unitService.CreateUnit(User, createUnitDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all units", Description = "Requires authentication")]
    public async Task<IActionResult> GetAllUnits(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? filterOn = null,
        [FromQuery] string? filterQuery = null,
        [FromQuery] string? sortBy = null)
    {
        var response = await _unitService.GetAllUnits(
            User, pageNumber, pageSize, filterOn, filterQuery, sortBy);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{unitId:guid}")]
    [SwaggerOperation(Summary = "Get unit details by ID", Description = "Requires authentication")]
    public async Task<IActionResult> GetUnitDetailsById(Guid unitId)
    {
        var response = await _unitService.GetUnitDetailsById(User, unitId);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{unitId:guid}")]
    [Authorize(Roles = "TEACHER")]
    [SwaggerOperation(Summary = "Update unit", Description = "Requires Teacher role")]
    public async Task<IActionResult> UpdateUnit([FromBody] UpdateUnitDto updateUnitDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _unitService.UpdateUnit(User, updateUnitDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{unitId:guid}")]
    [Authorize(Roles = "TEACHER")]
    [SwaggerOperation(Summary = "Delete unit (soft delete)", Description = "Requires Teacher role")]
    public async Task<IActionResult>? DeleteUnit([FromRoute] Guid unitId)
    {
        var response = await _unitService.DeleteUnit(User, unitId);
        return StatusCode(response.StatusCode, response);
    }
}