using Domain.Models.Coupons;
using Shared.Dtos.CouponDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.CouponSpecs
{
    public class CouponWithDetailsSpecification : BaseSpecifications<Coupon>
    {
        public CouponWithDetailsSpecification(CouponFilterDto filter)
            : base(BuildCriteria(filter))
        {
            AddInclude(c => c.Grade);
            AddInclude(c => c.Lesson);
            AddOrderByDescending(c => c.CreatedAt);
            ApplyPaging(
                skip: (filter.PageIndex - 1) * filter.PageSize,
                take: filter.PageSize
            );
        }

        public CouponWithDetailsSpecification(int id)
            : base(c => c.Id == id)
        {
            AddInclude(c => c.Grade);
            AddInclude(c => c.Lesson!);
        }

        internal static Expression<Func<Coupon, bool>> BuildCriteria(CouponFilterDto filter)
        {
            var now = DateTime.UtcNow;

            return c =>
                (!filter.GradeId.HasValue || c.GradeId == filter.GradeId.Value) &&
                (!filter.LessonId.HasValue || c.LessonId == filter.LessonId.Value) &&
                (string.IsNullOrEmpty(filter.Code) || c.Code.Contains(filter.Code)) &&
                (
                    !filter.Status.HasValue ||
                    (filter.Status == CouponStatus.Expired &&
                        c.ExpiryDate.HasValue && c.ExpiryDate.Value < now) ||
                    (filter.Status == CouponStatus.Exhausted &&
                        c.UsedCount >= c.MaxUsage &&
                        (!c.ExpiryDate.HasValue || c.ExpiryDate.Value >= now)) ||
                    (filter.Status == CouponStatus.Inactive &&
                        !c.IsActive &&
                        c.UsedCount < c.MaxUsage &&
                        (!c.ExpiryDate.HasValue || c.ExpiryDate.Value >= now)) ||
                    (filter.Status == CouponStatus.Active &&
                        c.IsActive &&
                        c.UsedCount < c.MaxUsage &&
                        (!c.ExpiryDate.HasValue || c.ExpiryDate.Value >= now))
                );
        }
    }
}
