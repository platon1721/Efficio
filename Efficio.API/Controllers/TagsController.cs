using Efficio.Core.Application.DTOs;
using Efficio.Core.Application.DTOs.Create;
using Efficio.Core.Application.DTOs.Update;
using Efficio.Core.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Asp.Versioning;

namespace Efficio.API.Controllers;

[ApiVersion(1.0)]
[Authorize]
public class TagsController : BaseApiController
{
    private readonly ITagService _tagService;

    public TagsController(ITagService tagService)
    {
        _tagService = tagService;
    }

    /// <summary>
    /// Tagastab kõik märksõnad
    /// </summary>
    /// <returns>Märksõnade nimekiri</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TagDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _tagService.GetAllAsync();
        return Ok(result.Data);
    }

    /// <summary>
    /// Tagastab konkreetse märksõna
    /// </summary>
    /// <param name="id">Märksõna ID</param>
    /// <returns>Märksõna andmed</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TagDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _tagService.GetByIdAsync(id);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Tagastab märksõna nime järgi
    /// </summary>
    /// <param name="title">Märksõna nimi</param>
    /// <returns>Märksõna andmed</returns>
    [HttpGet("by-title/{title}")]
    [ProducesResponseType(typeof(TagDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByTitle(string title)
    {
        var result = await _tagService.GetByTitleAsync(title);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Tagastab märksõnad teatud tagasiside jaoks
    /// </summary>
    /// <param name="feedbackId">Tagasiside ID</param>
    /// <returns>Märksõnade nimekiri</returns>
    [HttpGet("by-feedback/{feedbackId}")]
    [ProducesResponseType(typeof(IEnumerable<TagDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByFeedback(Guid feedbackId)
    {
        var result = await _tagService.GetByFeedbackAsync(feedbackId);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Tagastab märksõnad teatud postituse jaoks
    /// </summary>
    /// <param name="postId">Postituse ID</param>
    /// <returns>Märksõnade nimekiri</returns>
    [HttpGet("by-post/{postId}")]
    [ProducesResponseType(typeof(IEnumerable<TagDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByPost(Guid postId)
    {
        var result = await _tagService.GetByPostAsync(postId);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Loob uue märksõna
    /// </summary>
    /// <param name="createDto">Märksõna loomise andmed</param>
    /// <returns>Loodud märksõna andmed</returns>
    [HttpPost]
    [ProducesResponseType(typeof(TagDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTagDto createDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            return Unauthorized();

        var result = await _tagService.CreateAsync(createDto, userGuid);

        if (result.Success)
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);

        return BadRequest(result.Message);
    }

    /// <summary>
    /// Uuendab märksõna andmeid
    /// </summary>
    /// <param name="id">Märksõna ID</param>
    /// <param name="updateDto">Uuendamise andmed</param>
    /// <returns>Uuendatud märksõna andmed</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TagDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTagDto updateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            return Unauthorized();

        var result = await _tagService.UpdateAsync(id, updateDto, userGuid);

        if (result.Success)
            return Ok(result.Data);

        return result.Message.Contains("not found") ? NotFound(result.Message) : BadRequest(result.Message);
    }

    /// <summary>
    /// Kustutab märksõna
    /// </summary>
    /// <param name="id">Märksõna ID</param>
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

        var result = await _tagService.DeleteAsync(id, userGuid);

        if (result.Success)
            return NoContent();

        return result.Message.Contains("not found") ? NotFound(result.Message) : BadRequest(result.Message);
    }
}