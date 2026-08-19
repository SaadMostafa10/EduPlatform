using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Coupons
{
    public class CouponStatsProjection
    {
        public int TotalCount { get; set; }
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public int ExpiredCount { get; set; }
        public int TotalUsages { get; set; }
        public int RemainingUsages { get; set; }
    }
}
