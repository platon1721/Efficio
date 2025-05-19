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
public class FeedbacksController : BaseApiController
{
    private readonly IFeedbackService _feedbackService;

    public FeedbacksController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    /// <summary>
    /// Tagastab kõik tagasisided
    /// </summary>
    /// <returns>Tagasisidede nimekiri</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FeedbackDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _feedbackService.GetAllAsync();
        return Ok(result.Data);
    }

    /// <summary>
    /// Tagastab konkreetse tagasiside
    /// </summary>
    /// <param name="id">Tagasiside ID</param>
    /// <returns>Tagasiside andmed</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(FeedbackDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _feedbackService.GetByIdAsync(id);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Tagastab tagasiside koos kommentaaridega
    /// </summary>
    /// <param name="id">Tagasiside ID</param>
    /// <returns>Tagasiside andmed koos kommentaaridega</returns>
    [HttpGet("{id}/comments")]
    [ProducesResponseType(typeof(FeedbackDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWithComments(Guid id)
    {
        var result = await _feedbackService.GetWithCommentsAsync(id);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Tagastab tagasiside koos märksõnadega
    /// </summary>
    /// <param name="id">Tagasiside ID</param>
    /// <returns>Tagasiside andmed koos märksõnadega</returns>
    [HttpGet("{id}/tags")]
    [ProducesResponseType(typeof(FeedbackDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWithTags(Guid id)
    {
        var result = await _feedbackService.GetWithTagsAsync(id);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Tagastab autori järgi tagasisided
    /// </summary>
    /// <param name="authorId">Autori ID</param>
    /// <returns>Tagasisidede nimekiri</returns>
    [HttpGet("by-author/{authorId}")]
    [ProducesResponseType(typeof(IEnumerable<FeedbackDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByAuthor(Guid authorId)
    {
        var result = await _feedbackService.GetByAuthorAsync(authorId);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Tagastab märksõna järgi tagasisided
    /// </summary>
    /// <param name="tagId">Märksõna ID</param>
    /// <returns>Tagasisidede nimekiri</returns>
    [HttpGet("by-tag/{tagId}")]
    [ProducesResponseType(typeof(IEnumerable<FeedbackDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByTag(Guid tagId)
    {
        var result = await _feedbackService.GetByTagAsync(tagId);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Loob uue tagasiside
    /// </summary>
    /// <param name="createDto">Tagasiside loomise andmed</param>
    /// <returns>Loodud tagasiside andmed</returns>
    [HttpPost]
    [ProducesResponseType(typeof(FeedbackDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateFeedbackDto createDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            return Unauthorized();

        var result = await _feedbackService.CreateAsync(createDto, userGuid);

        if (result.Success)
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);

        return BadRequest(result.Message);
    }

    /// <summary>
    /// Uuendab tagasiside andmeid
    /// </summary>
    /// <param name="id">Tagasiside ID</param>
    /// <param name="updateDto">Uuendamise andmed</param>
    /// <returns>Uuendatud tagasiside andmed</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(FeedbackDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateFeedbackDto updateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            return Unauthorized();

        var result = await _feedbackService.UpdateAsync(id, updateDto, userGuid);

        if (result.Success)
            return Ok(result.Data);

        return result.Message.Contains("not found") ? NotFound(result.Message) : BadRequest(result.Message);
    }

    /// <summary>
    /// Kustutab tagasiside
    /// </summary>
    /// <param name="id">Tagasiside ID</param>
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

        var result = await _feedbackService.DeleteAsync(id, userGuid);

        if (result.Success)
            return NoContent();

        return result.Message.Contains("not found") ? NotFound(result.Message) : BadRequest(result.Message);
    }
}