using Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.Auth
{
    public class ActiveOtpByUserAndCodeSpecification : BaseSpecifications<PasswordResetOtp>
    {
        public ActiveOtpByUserAndCodeSpecification(string userId, string code)
            : base(o => o.UserId == userId && o.Code == code && !o.IsUsed)
        {
        }
    }
}
