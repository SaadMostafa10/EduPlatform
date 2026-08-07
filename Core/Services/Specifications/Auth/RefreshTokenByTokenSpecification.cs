using Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.Auth
{
    public class RefreshTokenByTokenSpecification : BaseSpecifications<RefreshToken>
    {
        public RefreshTokenByTokenSpecification(string token)
            : base(rt => rt.Token == token)
        {
        }
    }
}
