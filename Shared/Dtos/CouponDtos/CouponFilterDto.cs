using Domain.Models.Coupons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.CouponDtos
{
    public class CouponFilterDto
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public int? GradeId { get; set; }
        public int? LessonId { get; set; }
        public CouponStatus? Status { get; set; } //
        public string? Code { get; set; }
    }
}
