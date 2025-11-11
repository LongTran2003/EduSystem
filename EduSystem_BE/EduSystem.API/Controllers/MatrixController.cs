using EduSystem.Models.DTOs.Matrix;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EduSystem.API.Controllers
{
    [ApiController]
    [Route("api/matrices")]
    [SwaggerTag("Matrix Management APIs")]

    public class MatrixController : ControllerBase
    {
        private readonly IMatrixService _matrixService;

        public MatrixController(IMatrixService matrixService)
        {
            _matrixService = matrixService;
        }

        [HttpPost()]
        [Authorize(Roles = "ADMIN,TEACHER")]
        [SwaggerOperation(Summary = "Create a new matrix", Description = "Requires ADMIN or TEACHER role")]
        public async Task<IActionResult> CreateMatrix([FromBody] CreateMatrixDto createMatrixDto)
        {
            var result = await _matrixService.CreateMatrix(User, createMatrixDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{matrixId:guid}")]
        [Authorize(Roles = "TEACHER")]
        [SwaggerOperation(Summary = "Update an existing matrix", Description = "Requires TEACHER role")]
        public async Task<IActionResult> UpdateMatrix([FromBody] UpdateMatrixDto updateMatrixDto)
        {
            var result = await _matrixService.UpdateMatrix(User, updateMatrixDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet()]
        [SwaggerOperation(Summary = "Get all matrices", Description = "Supports pagination, filtering, and sorting")]
        public async Task<IActionResult> GetAllMatrices(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] string? filterOn = null, 
            [FromQuery] string? filterQuery = null, 
            [FromQuery] string? sortBy = null)
        {
            var result = await _matrixService.GetAllMatrices(User, pageNumber, pageSize, filterOn, filterQuery, sortBy);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{matrixId:guid}")]
        [SwaggerOperation(Summary = "Get matrix by ID", Description = "Fetches a specific matrix by its ID")]
        public async Task<IActionResult> GetMatrixById(Guid matrixId)
        {
            var result = await _matrixService.GetMatrixById(User, matrixId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{matrixId:guid}")]
        [Authorize(Roles = "ADMIN,TEACHER")]
        [SwaggerOperation(Summary = "Delete a matrix", Description = "Requires ADMIN or TEACHER role")]
        public async Task<IActionResult> DeleteMatrix(Guid id)
        {
            var result = await _matrixService.DeleteMatrix(User, id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
