// Efficio.Core/Application/Services/Interfaces/IAuthService.cs
using Efficio.Core.Application.DTOs.Auth;
using Efficio.Core.Application.DTOs.Base;

namespace Efficio.Core.Application.Services.Interfaces;

public interface IAuthService
{
    Task<BaseResponse<TokenDto>> LoginAsync(LoginDto loginDto);
    Task<BaseResponse<TokenDto>> RegisterAsync(RegisterDto registerDto);
    Task<BaseResponse<TokenDto>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
    Task<BaseResponse<bool>> RevokeTokenAsync(string refreshToken);
}