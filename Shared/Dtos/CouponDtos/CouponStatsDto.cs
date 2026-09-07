using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.CouponDtos
{
    public class CouponStatsDto
    {
        public int TotalCount { get; set; }
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public int ExpiredCount { get; set; }
        public int TotalUsages { get; set; }
        public int RemainingUsages { get; set; }
        public double UsagePercentage { get; set; }
    }
}
