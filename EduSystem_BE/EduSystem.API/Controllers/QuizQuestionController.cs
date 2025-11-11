using EduSystem.Models.DTO;
using EduSystem.Models.DTOs.QuizQuestion;
using EduSystem.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduSystem.API.Controllers
{
    [ApiController]
    [Route("api/quiz-question")]

    public class QuizQuestionController : ControllerBase
    {
        private readonly IQuizQuestionService _quizQuestionService;

        public QuizQuestionController(IQuizQuestionService quizQuestionService)
        {
            _quizQuestionService = quizQuestionService;
        }

        [HttpPost]
        [Authorize(Roles = "TEACHER,ADMIN")]
        public async Task<ActionResult<ResponseDto>> AddQuestionToQuiz([FromBody] CreateQuizQuestionDto createQuizQuestionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _quizQuestionService.AddQuestionToQuiz(User, createQuizQuestionDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("bulk")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        public async Task<ActionResult<ResponseDto>> AddMultipleQuestionsToQuiz([FromBody] AddQuestionsToQuizDto addQuestionsToQuizDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _quizQuestionService.AddMultipleQuestionsToQuiz(User, addQuestionsToQuizDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("quiz/{quizId}")]
        public async Task<ActionResult<ResponseDto>> GetQuestionsByQuizId(Guid quizId)
        {
            var response = await _quizQuestionService.GetQuestionsByQuizId(User, quizId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("question/{questionId}")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        public async Task<ActionResult<ResponseDto>> GetQuizzesByQuestionId(Guid questionId)
        {
            var response = await _quizQuestionService.GetQuizzesByQuestionId(User, questionId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("order")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        public async Task<ActionResult<ResponseDto>> UpdateQuestionOrder([FromBody] UpdateQuestionOrderDto updateQuestionOrderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _quizQuestionService.UpdateQuestionOrder(User, updateQuestionOrderDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        [Authorize(Roles = "TEACHER,ADMIN")]
        public async Task<ActionResult<ResponseDto>> RemoveQuestionFromQuiz([FromQuery] Guid quizId, [FromQuery] Guid questionId)
        {
            var response = await _quizQuestionService.RemoveQuestionFromQuiz(User, quizId, questionId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("quiz/{quizId}")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        public async Task<ActionResult<ResponseDto>> RemoveAllQuestionsFromQuiz(Guid quizId)
        {
            var response = await _quizQuestionService.RemoveAllQuestionsFromQuiz(User, quizId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
