using Domain.Contracts;
using Domain.Models.Coupons;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class CouponRepository : GenericRepository<Coupon>, ICouponRepository
    {
        public CouponRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<CouponStatsProjection> GetStatsAsync()
        {
            var stats = await _context.Coupons
                .Where(c => !c.IsDeleted)
                .GroupBy(c => 1)
                .Select(g => new CouponStatsProjection
                {
                    TotalCount = g.Count(),
                    ActiveCount = g.Count(c => c.IsActive),
                    InactiveCount = g.Count(c => !c.IsActive),
                    ExpiredCount = g.Count(c => c.ExpiryDate != null && c.ExpiryDate < DateTime.UtcNow),
                    TotalUsages = g.Sum(c => c.UsedCount),
                    RemainingUsages = g.Sum(c => c.MaxUsage - c.UsedCount)
                })
                .FirstOrDefaultAsync();

            return stats ?? new CouponStatsProjection();
        }

        public async Task<bool> CodeExistsAsync(string code)
        {
            return await _context.Coupons.AnyAsync(c => c.Code == code);
        }
    }
}
