using Shared.Dtos.AuthDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);

        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);

        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);

        Task RevokeTokenAsync(RevokeTokenRequestDto request);

        Task ForgotPasswordAsync(ForgotPasswordRequestDto request);

        Task ResetPasswordAsync(ResetPasswordRequestDto request);
    }
}
