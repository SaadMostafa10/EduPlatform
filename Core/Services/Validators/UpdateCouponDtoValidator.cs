using FluentValidation;
using Shared.Dtos.CouponDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Validators
{
    public class UpdateCouponDtoValidator : AbstractValidator<UpdateCouponDto>
    {
        public UpdateCouponDtoValidator()
        {
            RuleFor(x => x.MaxUsage)
                .GreaterThan(0).WithMessage("Max usage must be greater than zero.");

            RuleFor(x => x.ExpiryDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Expiry date must be in the future.")
                .When(x => x.ExpiryDate.HasValue);
        }
    }
}
