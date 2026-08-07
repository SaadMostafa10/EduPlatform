using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Services.Abstractions;
using Services.Specifications.Auth;
using Shared.Dtos.AuthDtos;
using Shared.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly JwtOptions _jwtOptions;
        private readonly IEmailService _emailService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptions<JwtOptions> jwtOptions,
            IEmailService emailService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtOptions = jwtOptions.Value;
            _emailService = emailService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
                throw new BadRequestException("Email already exists.");

            var user = _mapper.Map<ApplicationUser>(request);

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.First().Description);

            return await GenerateAuthResponseAsync(user);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                throw new UnauthorizedException("Invalid email or password.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
                throw new UnauthorizedException("Invalid email or password.");

            return await GenerateAuthResponseAsync(user);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var spec = new RefreshTokenByTokenSpecification(request.RefreshToken);
            var refreshTokenRepo = _unitOfWork.Repository<RefreshToken>();

            var storedToken = await refreshTokenRepo.GetWithSpecAsync(spec);
            if (storedToken is null || !storedToken.IsActive)
                throw new UnauthorizedException("Refresh token is invalid or expired.");

            var user = await _userManager.FindByIdAsync(storedToken.UserId);
            if (user is null)
                throw new NotFoundException(nameof(ApplicationUser), storedToken.UserId);

            storedToken.RevokedOn = DateTime.UtcNow;
            refreshTokenRepo.Update(storedToken);

            return await GenerateAuthResponseAsync(user);
        }

        public async Task RevokeTokenAsync(RevokeTokenRequestDto request)
        {
            var spec = new RefreshTokenByTokenSpecification(request.RefreshToken);
            var refreshTokenRepo = _unitOfWork.Repository<RefreshToken>();

            var storedToken = await refreshTokenRepo.GetWithSpecAsync(spec);
            if (storedToken is null || !storedToken.IsActive)
                throw new NotFoundException(nameof(RefreshToken), request.RefreshToken);

            storedToken.RevokedOn = DateTime.UtcNow;
            refreshTokenRepo.Update(storedToken);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ForgotPasswordAsync(ForgotPasswordRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                throw new NotFoundException(nameof(ApplicationUser), request.Email);

            var otpCode = System.Security.Cryptography.RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

            var otp = new PasswordResetOtp
            {
                Code = otpCode,
                ExpiresOn = DateTime.UtcNow.AddMinutes(10),
                UserId = user.Id
            };

            await _unitOfWork.Repository<PasswordResetOtp>().AddAsync(otp);
            await _unitOfWork.SaveChangesAsync();

            await _emailService.SendEmailAsync(
                user.Email!,
                "Password Reset Code",
                $"Your password reset code is: {otpCode}. It expires in 10 minutes.");
        }

        public async Task ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                throw new NotFoundException(nameof(ApplicationUser), request.Email);

            var spec = new ActiveOtpByUserAndCodeSpecification(user.Id, request.Otp);
            var otpRepo = _unitOfWork.Repository<PasswordResetOtp>();

            var storedOtp = await otpRepo.GetWithSpecAsync(spec);
            if (storedOtp is null || !storedOtp.IsValid)
                throw new BadRequestException("Invalid or expired OTP.");

            var identityToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, identityToken, request.NewPassword);

            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.First().Description);

            storedOtp.IsUsed = true;
            otpRepo.Update(storedOtp);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<AuthResponseDto> GenerateAuthResponseAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email!),
                new("FullName", user.FullName)
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var (refreshTokenValue, refreshTokenExpiresOn) = _tokenService.GenerateRefreshToken();

            var refreshToken = new RefreshToken
            {
                Token = refreshTokenValue,
                ExpiresOn = refreshTokenExpiresOn,
                UserId = user.Id
            };

            await _unitOfWork.Repository<RefreshToken>().AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<AuthResponseDto>(user);
            response.IsAuthenticated = true;
            response.Token = accessToken;
            response.TokenExpiresOn = DateTime.UtcNow.AddMinutes(_jwtOptions.DurationInMinutes);
            response.RefreshToken = refreshTokenValue;
            response.RefreshTokenExpiresOn = refreshTokenExpiresOn;

            return response;
        }
    }
}
