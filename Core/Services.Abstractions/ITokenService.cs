using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);

        (string Token, DateTime ExpiresOn) GenerateRefreshToken();

        string CreateToken(ApplicationUser user, IList<string> roles);
    }
}
