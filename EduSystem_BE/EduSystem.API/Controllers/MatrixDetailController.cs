using EduSystem.Models.DTOs.MatrixDetail;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EduSystem.API.Controllers
{
    [ApiController]
    [Route("api/matrix-details")]
    [SwaggerTag("Matrix Detail Management APIs")]

    public class MatrixDetailController : ControllerBase
    {
        private readonly IMatrixDetailService _matrixDetailService;

        public MatrixDetailController(IMatrixDetailService matrixDetailService)
        {
            _matrixDetailService = matrixDetailService;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new matrix detail", Description = "Requires authentication")]
        public async Task<IActionResult> CreateMatrixDetail([FromBody] CreateMatrixDetailDto createMatrixDetailDto)
        {
            var result = await _matrixDetailService.CreateMatrixDetail(User, createMatrixDetailDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{matrixDetailId:guid}")]
        [SwaggerOperation(Summary = "Update an existing matrix detail", Description = "Requires authentication")]
        public async Task<IActionResult> UpdateMatrixDetail([FromBody] UpdateMatrixDetailDto updateMatrixDetailDto)
        {
            var result = await _matrixDetailService.UpdateMatrixDetail(User, updateMatrixDetailDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{matrixDetailId:guid}")]
        [SwaggerOperation(Summary = "Get matrix detail by ID", Description = "Fetches a specific matrix detail by its ID")]
        public async Task<IActionResult> GetMatrixDetail([FromRoute] Guid matrixDetailId)
        {
            var result = await _matrixDetailService.GetMatrixDetailById(User, matrixDetailId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all matrix details", Description = "Supports pagination, filtering, and sorting")]
        public async Task<IActionResult> GetAllMatrixDetails(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? filterOn = null,
            [FromQuery] string? filterQuery = null,
            [FromQuery] string? sortBy = null)
        {
            var result = await _matrixDetailService.GetAllMatrixDetails(
                User, pageNumber, pageSize, filterOn, filterQuery, sortBy);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{matrixDetailId:guid}")]
        [SwaggerOperation(Summary = "Delete a matrix detail", Description = "Requires authentication")]
        public async Task<IActionResult> DeleteMatrixDetail([FromRoute] Guid matrixDetailId)
        {
            var result = await _matrixDetailService.DeleteMatrixDetail(User, matrixDetailId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
