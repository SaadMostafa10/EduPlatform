using FluentValidation;
using Shared.Dtos.CouponDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Validators
{
    public class BulkGenerateCouponDtoValidator : AbstractValidator<BulkGenerateCouponDto>
    {
        public BulkGenerateCouponDtoValidator()
        {
            RuleFor(x => x.GradeId)
                .GreaterThan(0).WithMessage("Grade is required.");

            RuleFor(x => x.LessonId)
                .NotNull().WithMessage("Lesson is required.")
                .GreaterThan(0).WithMessage("Lesson is required.");

            RuleFor(x => x.Count)
                .GreaterThan(0).WithMessage("Count must be greater than zero.")
                .LessThanOrEqualTo(500).WithMessage("Cannot generate more than 500 coupons at once.");

            RuleFor(x => x.CodePrefix)
                .NotEmpty().WithMessage("Code prefix is required for bulk generation.")
                .MaximumLength(12).WithMessage("Code prefix must not exceed 12 characters.")
                .Matches("^[A-Za-z0-9]+$").WithMessage("Code prefix must contain only English letters and numbers.");

            RuleFor(x => x.MaxUsagePerCoupon)
                .GreaterThan(0).WithMessage("Max usage per coupon must be greater than zero.");

            RuleFor(x => x.ExpiryDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Expiry date must be in the future.")
                .When(x => x.ExpiryDate.HasValue);
        }
    }
}
