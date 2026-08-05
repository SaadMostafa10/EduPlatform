using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.AuthDtos
{
    public class AuthResponseDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiresOn { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiresOn { get; set; }
        public string? Message { get; set; }
    }
}
