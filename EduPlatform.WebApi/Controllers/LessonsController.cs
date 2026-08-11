using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using Shared.Constants;
using Shared.Dtos.LessonDtos;

namespace EduPlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonsController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationResponse<LessonResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllLessons([FromQuery] LessonSpecParams specParams)
        {
            var isStudent = User.IsInRole(UserRoles.Student);
            int? studentGradeId = null;
            if (isStudent)
            {
                var gradeClaim = User.FindFirst("GradeId")?.Value;
                studentGradeId = int.TryParse(gradeClaim, out var g) ? g : null;
            }
            var result = await _lessonService.GetAllLessonsAsync(specParams, isStudent, studentGradeId);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(LessonResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLessonById(int id)
        {
            var lesson = await _lessonService.GetLessonByIdAsync(id);
            if (lesson is null)
                return NotFound(new { message = $"Lesson with ID {id} was not found." });

            return Ok(lesson);
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.Teacher},{UserRoles.Admin}")] 
        [ProducesResponseType(typeof(LessonResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLesson([FromBody] CreateLessonRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdLesson = await _lessonService.CreateLessonAsync(request);
            return CreatedAtAction(nameof(GetLessonById), new { id = createdLesson.Id }, createdLesson);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles =$"{UserRoles.Teacher},{UserRoles.Admin}" )] 
        [ProducesResponseType(typeof(LessonResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateLesson(int id, [FromBody] UpdateLessonRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedLesson = await _lessonService.UpdateLessonAsync(id, request);
            if (updatedLesson is null)
                return NotFound(new { message = $"Lesson with ID {id} was not found." });

            return Ok(updatedLesson);
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{UserRoles.Teacher},{UserRoles.Admin}")] 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var success = await _lessonService.DeleteLessonAsync(id);
            if (!success)
                return NotFound(new { message = $"Lesson with ID {id} was not found or already deleted." });

            return NoContent();
        }
    }
}
