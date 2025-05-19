using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Efficio.Core.Application.DTOs.Auth;
using Efficio.Core.Application.DTOs.Base;
using Efficio.Core.Application.Services.Interfaces;
using Efficio.Core.Domain.Entities.Common;
using Efficio.Core.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Efficio.Core.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<BaseResponse<TokenDto>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            // Find user by email
            var user = await _unitOfWork.Users.GetUserByEmailAsync(loginDto.Email);
            if (user == null || user is not AuthUser authUser)
            {
                return BaseResponse<TokenDto>.FailResult("Invalid email or password.");
            }
            
            // Verify password
            if (!VerifyPassword(loginDto.Password, authUser.PasswordHash, authUser.Salt))
            {
                return BaseResponse<TokenDto>.FailResult("Invalid email or password.");
            }
            
            // Generate JWT token
            var token = GenerateJwtToken(authUser);
            
            // Generate refresh token
            var refreshToken = GenerateRefreshToken();
            authUser.RefreshToken = refreshToken;
            authUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Token expires in 7 days
            
            await _unitOfWork.Users.UpdateAsync(authUser);
            await _unitOfWork.CompleteAsync();
            
            return BaseResponse<TokenDto>.SuccessResult(new TokenDto
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(60) // Same as token expiry
            }, "Login successful.");
        }
        catch (Exception ex)
        {
            return BaseResponse<TokenDto>.FailResult($"Error during login: {ex.Message}");
        }
    }

    public async Task<BaseResponse<TokenDto>> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            // Check if email already exists
            var existingUser = await _unitOfWork.Users.GetUserByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return BaseResponse<TokenDto>.FailResult($"User with email {registerDto.Email} already exists.");
            }
            
            // Validate passwords match
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return BaseResponse<TokenDto>.FailResult("Passwords do not match.");
            }
            
            // Create salt and hash password
            var salt = GenerateSalt();
            var passwordHash = HashPassword(registerDto.Password, salt);
            
            // Create user
            var authUser = new AuthUser
            {
                FirstName = registerDto.FirstName,
                SurName = registerDto.SurName,
                Email = registerDto.Email,
                CountryCode = registerDto.CountryCode,
                Number = registerDto.Number,
                PasswordHash = passwordHash,
                Salt = Convert.ToBase64String(salt)
            };
            
            await _unitOfWork.Users.AddAsync(authUser);
            await _unitOfWork.CompleteAsync();
            
            // Generate JWT token
            var token = GenerateJwtToken(authUser);
            
            // Generate refresh token
            var refreshToken = GenerateRefreshToken();
            authUser.RefreshToken = refreshToken;
            authUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            
            await _unitOfWork.Users.UpdateAsync(authUser);
            await _unitOfWork.CompleteAsync();
            
            return BaseResponse<TokenDto>.SuccessResult(new TokenDto
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(60)
            }, "Registration successful.");
        }
        catch (Exception ex)
        {
            return BaseResponse<TokenDto>.FailResult($"Error during registration: {ex.Message}");
        }
    }

    public async Task<BaseResponse<TokenDto>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
    {
        try
        {
            // Validate access token
            var principal = GetPrincipalFromExpiredToken(refreshTokenDto.AccessToken);
            if (principal == null)
            {
                return BaseResponse<TokenDto>.FailResult("Invalid access token.");
            }
            
            // Extract user ID from token
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                return BaseResponse<TokenDto>.FailResult("Invalid access token.");
            }
            
            // Find user
            var user = await _unitOfWork.Users.GetByIdAsync(userGuid);
            if (user == null || user is not AuthUser authUser)
            {
                return BaseResponse<TokenDto>.FailResult("User not found.");
            }
            
            // Validate refresh token
            if (authUser.RefreshToken != refreshTokenDto.RefreshToken || 
                authUser.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return BaseResponse<TokenDto>.FailResult("Invalid or expired refresh token.");
            }
            
            // Generate new JWT token
            var newToken = GenerateJwtToken(authUser);
            
            // Generate new refresh token
            var newRefreshToken = GenerateRefreshToken();
            authUser.RefreshToken = newRefreshToken;
            authUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            
            await _unitOfWork.Users.UpdateAsync(authUser);
            await _unitOfWork.CompleteAsync();
            
            return BaseResponse<TokenDto>.SuccessResult(new TokenDto
            {
                AccessToken = newToken,
                RefreshToken = newRefreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(60)
            }, "Token refreshed successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<TokenDto>.FailResult($"Error refreshing token: {ex.Message}");
        }
    }

    public async Task<BaseResponse<bool>> RevokeTokenAsync(string refreshToken)
    {
        try
        {
            // Find user by refresh token
            var users = await _unitOfWork.Users.FindAsync(u => u is AuthUser && ((AuthUser)u).RefreshToken == refreshToken);
            var authUser = users.FirstOrDefault() as AuthUser;
            
            if (authUser == null)
            {
                return BaseResponse<bool>.FailResult("Invalid refresh token.");
            }
            
            // Revoke token
            authUser.RefreshToken = null;
            authUser.RefreshTokenExpiryTime = null;
            
            await _unitOfWork.Users.UpdateAsync(authUser);
            await _unitOfWork.CompleteAsync();
            
            return BaseResponse<bool>.SuccessResult(true, "Token revoked successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.FailResult($"Error revoking token: {ex.Message}");
        }
    }
    
    #region Private Helper Methods
    
    private string GenerateJwtToken(AuthUser user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.SurName}"),
            new Claim(ClaimTypes.Email, user.Email),
            // Add roles and other claims as needed
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(60), // Token expires in 60 minutes
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
    
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    
    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = false // We don't care about expiry here
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, 
                StringComparison.InvariantCultureIgnoreCase))
        {
            return null;
        }

        return principal;
    }
    
    private byte[] GenerateSalt()
    {
        var salt = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }
    
    private string HashPassword(string password, byte[] salt)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32);
        return Convert.ToBase64String(hash);
    }
    
    private bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        var salt = Convert.FromBase64String(storedSalt);
        var hash = HashPassword(password, salt);
        return hash == storedHash;
    }
    
    #endregion
}