// Efficio.API/Controllers/UsersController.cs
using Efficio.Core.Application.DTOs;
using Efficio.Core.Application.DTOs.Create;
using Efficio.Core.Application.DTOs.Update;
using Efficio.Core.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Efficio.API.Controllers;

[ApiVersion("1.0")]
[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Tagastab kõik kasutajad
    /// </summary>
    /// <returns>Kasutajate nimekiri</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllAsync();
        return Ok(result.Data);
    }

    /// <summary>
    /// Tagastab konkreetse kasutaja
    /// </summary>
    /// <param name="id">Kasutaja ID</param>
    /// <returns>Kasutaja andmed</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _userService.GetByIdAsync(id);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Tagastab kasutaja koos tema osakondadega
    /// </summary>
    /// <param name="id">Kasutaja ID</param>
    /// <returns>Kasutaja andmed koos osakondadega</returns>
    [HttpGet("{id}/departments")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserWithDepartments(Guid id)
    {
        var result = await _userService.GetUserWithDepartmentsAsync(id);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Loob uue kasutaja
    /// </summary>
    /// <param name="createDto">Kasutaja loomise andmed</param>
    /// <returns>Loodud kasutaja andmed</returns>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUserDto createDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            return Unauthorized();

        var result = await _userService.CreateAsync(createDto, userGuid);

        if (result.Success)
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);

        return BadRequest(result.Message);
    }

    /// <summary>
    /// Uuendab kasutaja andmeid
    /// </summary>
    /// <param name="id">Kasutaja ID</param>
    /// <param name="updateDto">Uuendamise andmed</param>
    /// <returns>Uuendatud kasutaja andmed</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto updateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            return Unauthorized();

        var result = await _userService.UpdateAsync(id, updateDto, userGuid);

        if (result.Success)
            return Ok(result.Data);

        return result.Message.Contains("not found") ? NotFound(result.Message) : BadRequest(result.Message);
    }

    /// <summary>
    /// Kustutab kasutaja
    /// </summary>
    /// <param name="id">Kasutaja ID</param>
    /// <returns>Operatsiooni tulemus</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            return Unauthorized();

        var result = await _userService.DeleteAsync(id, userGuid);

        if (result.Success)
            return NoContent();

        return result.Message.Contains("not found") ? NotFound(result.Message) : BadRequest(result.Message);
    }
}