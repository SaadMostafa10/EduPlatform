using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.AuthDtos;

namespace EduPlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradesController(IGradeService gradeService) => _gradeService = gradeService;

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<GradeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllGrades()
        {
            var grades = await _gradeService.GetAllWithSpecAsync();
            return Ok(grades);
        }
    }
}
