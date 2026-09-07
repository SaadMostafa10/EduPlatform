using FluentValidation;
using Shared.Dtos.CouponDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Validators
{
    public class CreateCouponDtoValidator : AbstractValidator<CreateCouponDto>
    {
        public CreateCouponDtoValidator()
        {
            RuleFor(x => x.GradeId)
                .GreaterThan(0).WithMessage("Grade is required.");

            RuleFor(x => x.LessonId)
                .NotNull().WithMessage("Lesson is required.")
                .GreaterThan(0).WithMessage("Lesson is required.");

            When(x => !x.AutoGenerateCode, () =>
            {
                RuleFor(x => x.ManualCode)
                    .NotEmpty().WithMessage("Manual coupon code is required.")
                    .MinimumLength(5).WithMessage("Coupon code must be at least 5 characters.")
                    .MaximumLength(20).WithMessage("Coupon code must not exceed 20 characters.")
                    .Matches("^[A-Za-z0-9]+$").WithMessage("Coupon code must contain only English letters and numbers.");
            });

            When(x => x.AutoGenerateCode, () =>
            {
                RuleFor(x => x.CodePrefix)
                    .MaximumLength(10).WithMessage("Code prefix must not exceed 10 characters.")
                    .Matches("^[A-Za-z0-9]*$").WithMessage("Code prefix must contain only English letters and numbers.")
                    .When(x => !string.IsNullOrEmpty(x.CodePrefix));
            });

            RuleFor(x => x.MaxUsage)
                .GreaterThan(0).WithMessage("Max usage must be greater than zero.");

            RuleFor(x => x.ExpiryDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Expiry date must be in the future.")
                .When(x => x.ExpiryDate.HasValue);

        }
    }
}
