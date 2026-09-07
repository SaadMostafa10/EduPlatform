using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.CouponDtos
{
    public class CreateCouponDto
    {
        public int GradeId { get; set; }
        public int LessonId { get; set; }

        public bool AutoGenerateCode { get; set; } = true;
        public string? CodePrefix { get; set; }
        public string? ManualCode { get; set; }

        public int MaxUsage { get; set; } = 25;
        public DateTime? ExpiryDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
