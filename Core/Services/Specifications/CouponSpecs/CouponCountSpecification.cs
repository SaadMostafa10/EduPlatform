using Domain.Models.Coupons;
using Shared.Dtos.CouponDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.CouponSpecs
{
    public class CouponCountSpecification : BaseSpecifications<Coupon>
    {
        public CouponCountSpecification(CouponFilterDto filter)
            : base(CouponWithDetailsSpecification.BuildCriteria(filter))
        {
        }
    }
}
