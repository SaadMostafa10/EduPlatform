using Domain.Models.Coupons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Dtos.CouponDtos
{
    public class CouponDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public int GradeId { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public int? LessonId { get; set; }
        public string LessonTitle { get; set; } = string.Empty;
        public int MaxUsage { get; set; }
        public int UsedCount { get; set; }
        public int RemainingUsages => Math.Max(0, MaxUsage - UsedCount);
        public DateTime? ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CouponStatus Status
        {
            get
            {
                if (ExpiryDate.HasValue && ExpiryDate.Value < DateTime.UtcNow)
                    return CouponStatus.Expired;

                if (UsedCount >= MaxUsage)
                    return CouponStatus.Exhausted;

                if (!IsActive)
                    return CouponStatus.Inactive;

                return CouponStatus.Active;
            }
        }
    }
}
