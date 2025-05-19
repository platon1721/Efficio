using Efficio.Core.Application.DTOs.Auth;
using Efficio.Core.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Asp.Versioning;

namespace Efficio.API.Controllers;

[ApiVersion(1.0)]
public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registreerib uue kasutaja
    /// </summary>
    /// <param name="registerDto">Registreerimise andmed</param>
    /// <returns>Autentimise token</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var result = await _authService.RegisterAsync(registerDto);

        if (result.Success)
            return Ok(result.Data);

        return BadRequest(result.Message);
    }

    /// <summary>
    /// Logib kasutaja sisse
    /// </summary>
    /// <param name="loginDto">Sisselogimise andmed</param>
    /// <returns>Autentimise token</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);

        if (result.Success)
            return Ok(result.Data);

        return Unauthorized(result.Message);
    }

    /// <summary>
    /// Värskendab autentimise tokenit
    /// </summary>
    /// <param name="refreshTokenDto">Vana token ja värskendamise token</param>
    /// <returns>Uus autentimise token</returns>
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        var result = await _authService.RefreshTokenAsync(refreshTokenDto);

        if (result.Success)
            return Ok(result.Data);

        return Unauthorized(result.Message);
    }

    /// <summary>
    /// Tühistab värskendamise tokeni
    /// </summary>
    /// <param name="refreshToken">Värskendamise token</param>
    /// <returns>Operatsiooni tulemus</returns>
    [HttpPost("revoke-token")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RevokeToken([FromBody] string refreshToken)
    {
        var result = await _authService.RevokeTokenAsync(refreshToken);

        if (result.Success)
            return Ok();

        return BadRequest(result.Message);
    }
}