using EduSystem.Models.DTOs.Matrix;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduSystem.API.Controllers
{
    [ApiController]
    [Route("api/matrix")]

    public class MatrixController : ControllerBase
    {
        private readonly IMatrixService _matrixService;

        public MatrixController(IMatrixService matrixService)
        {
            _matrixService = matrixService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "ADMIN,TEACHER")]
        public async Task<IActionResult> CreateMatrix([FromBody] CreateMatrixDto createMatrixDto)
        {
            var result = await _matrixService.CreateMatrix(User, createMatrixDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("update")]
        [Authorize(Roles = "TEACHER")]
        public async Task<IActionResult> UpdateMatrix([FromBody] UpdateMatrixDto updateMatrixDto)
        {
            var result = await _matrixService.UpdateMatrix(User, updateMatrixDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get/all")]
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

        [HttpGet("get/{matrixId:guid}")]
        public async Task<IActionResult> GetMatrixById(Guid matrixId)
        {
            var result = await _matrixService.GetMatrixById(User, matrixId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("delete/{matrixId:guid}")]
        [Authorize(Roles = "ADMIN, TEACHER")]
        public async Task<IActionResult> DeleteMatrix(Guid id)
        {
            var result = await _matrixService.DeleteMatrix(User, id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
