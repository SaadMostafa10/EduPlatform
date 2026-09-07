using Domain.Models.Common;
using Domain.Models.Identity;
using Domain.Models.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Coupons
{
    public class Coupon : AuditableEntity
    {
        public string Code { get; set; } = string.Empty;

        public int GradeId { get; set; }
        public Grade Grade { get; set; } = null!;

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;

        public int MaxUsage { get; set; }
        public int UsedCount { get; set; } = 0;

        public DateTime? ExpiryDate { get; set; }
        public bool IsActive { get; set; } = true;

        public string CreatedByUserId { get; set; } = string.Empty;
        public ApplicationUser CreatedByUser { get; set; } = null!;
    }
}
