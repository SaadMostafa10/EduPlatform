using Domain.Models.Coupons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ICouponRepository : IGenericRepository<Coupon>
    {
        Task<CouponStatsProjection> GetStatsAsync();
        Task<bool> CodeExistsAsync(string code);
    }
}
