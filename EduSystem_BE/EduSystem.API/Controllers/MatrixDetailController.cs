using EduSystem.Models.DTOs.MatrixDetail;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace EduSystem.API.Controllers
{
    [ApiController]
    [Route("api/matrix-detail")]

    public class MatrixDetailController : ControllerBase
    {
        private readonly IMatrixDetailService _matrixDetailService;

        public MatrixDetailController(IMatrixDetailService matrixDetailService)
        {
            _matrixDetailService = matrixDetailService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateMatrixDetail([FromBody] CreateMatrixDetailDto createMatrixDetailDto)
        {
            var result = await _matrixDetailService.CreateMatrixDetail(User, createMatrixDetailDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateMatrixDetail([FromBody] UpdateMatrixDetailDto updateMatrixDetailDto)
        {
            var result = await _matrixDetailService.UpdateMatrixDetail(User, updateMatrixDetailDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get/{id:guid}")]
        public async Task<IActionResult> GetMatrixDetail([FromRoute] Guid id)
        {
            var result = await _matrixDetailService.GetMatrixDetailById(User, id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get/all")]
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

        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> DeleteMatrixDetail([FromRoute] Guid id)
        {
            var result = await _matrixDetailService.DeleteMatrixDetail(User, id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
