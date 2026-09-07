using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.CouponDtos
{
    public class BulkGenerateCouponDto
    {
        public int GradeId { get; set; }
        public int LessonId { get; set; }

        public int Count { get; set; }
        public string CodePrefix { get; set; } = string.Empty;

        public int MaxUsagePerCoupon { get; set; } = 1;
        public DateTime? ExpiryDate { get; set; }
        public bool ActivateImmediately { get; set; } = true;
    }
}
