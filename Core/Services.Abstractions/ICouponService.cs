using Shared;
using Shared.Dtos.CouponDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface ICouponService
    {
        Task<PaginationResponse<CouponDto>> GetAllAsync(CouponFilterDto filter);
        Task<CouponDto> GetByIdAsync(int id);
        Task<CouponStatsDto> GetStatsAsync();
        Task<CouponDto> CreateAsync(CreateCouponDto dto, string createdByUserId);
        Task<IReadOnlyList<CouponDto>> BulkGenerateAsync(BulkGenerateCouponDto dto, string createdByUserId);
        Task<CouponDto> UpdateAsync(int id, UpdateCouponDto dto);
        Task UpdateStatusAsync(int id, bool isActive);
        Task DeleteAsync(int id);
    }
}
