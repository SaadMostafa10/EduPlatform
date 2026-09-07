using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using Shared.Dtos.CouponDtos;
using System.Security.Claims;

namespace EduPlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Teacher")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponsController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResponse<CouponDto>>> GetAll([FromQuery] CouponFilterDto filter)
        {
            var result = await _couponService.GetAllAsync(filter);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CouponDto>> GetById(int id)
        {
            var coupon = await _couponService.GetByIdAsync(id);
            return Ok(coupon);
        }

        [HttpGet("stats")]
        public async Task<ActionResult<CouponStatsDto>> GetStats()
        {
            var stats = await _couponService.GetStatsAsync();
            return Ok(stats);
        }

        [HttpPost]
        public async Task<ActionResult<CouponDto>> Create([FromBody] CreateCouponDto dto)
        {
            var userId = GetCurrentUserId();
            var created = await _couponService.CreateAsync(dto, userId);
            return Ok(created);
        }

        [HttpPost("bulk-generate")]
        public async Task<ActionResult<IReadOnlyList<CouponDto>>> BulkGenerate(
            [FromBody] BulkGenerateCouponDto dto)
        {
            var userId = GetCurrentUserId();
            var result = await _couponService.BulkGenerateAsync(dto, userId);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CouponDto>> Update(int id, [FromBody] UpdateCouponDto dto)
        {
            var result = await _couponService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateCouponStatusDto dto)
        {
            await _couponService.UpdateStatusAsync(id, dto.IsActive);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _couponService.DeleteAsync(id);
            return NoContent();
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }
    
    }
}
