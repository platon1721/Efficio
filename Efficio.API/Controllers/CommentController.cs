using Efficio.Core.Application.DTOs;
using Efficio.Core.Application.DTOs.Create;
using Efficio.Core.Application.DTOs.Update;
using Efficio.Core.Application.Services.Interfaces;
using Efficio.Core.Domain.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Asp.Versioning;

namespace Efficio.API.Controllers;

[ApiVersion(1.0)]
[Authorize]
public class CommentsController : BaseApiController
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    /// <summary>
    /// Tagastab kõik kommentaarid
    /// </summary>
    /// <returns>Kommentaaride nimekiri</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CommentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _commentService.GetAllAsync();
        return Ok(result.Data);
    }

    /// <summary>
    /// Tagastab konkreetse kommentaari
    /// </summary>
    /// <param name="id">Kommentaari ID</param>
    /// <returns>Kommentaari andmed</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _commentService.GetByIdAsync(id);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Tagastab autori järgi kommentaarid
    /// </summary>
    /// <param name="authorId">Autori ID</param>
    /// <returns>Kommentaaride nimekiri</returns>
    [HttpGet("by-author/{authorId}")]
    [ProducesResponseType(typeof(IEnumerable<CommentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByAuthor(Guid authorId)
    {
        var result = await _commentService.GetByAuthorAsync(authorId);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Tagastab tagasiside järgi kommentaarid
    /// </summary>
    /// <param name="feedbackId">Tagasiside ID</param>
    /// <returns>Kommentaaride nimekiri</returns>
    [HttpGet("by-feedback/{feedbackId}")]
    [ProducesResponseType(typeof(IEnumerable<CommentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByFeedback(Guid feedbackId)
    {
        var result = await _commentService.GetByCommentableAsync(CommentableEntityType.Feedback, feedbackId);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Tagastab postituse järgi kommentaarid
    /// </summary>
    /// <param name="postId">Postituse ID</param>
    /// <returns>Kommentaaride nimekiri</returns>
    [HttpGet("by-post/{postId}")]
    [ProducesResponseType(typeof(IEnumerable<CommentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByPost(Guid postId)
    {
        var result = await _commentService.GetByCommentableAsync(CommentableEntityType.Post, postId);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Loob uue kommentaari
    /// </summary>
    /// <param name="createDto">Kommentaari loomise andmed</param>
    /// <returns>Loodud kommentaari andmed</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCommentDto createDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            return Unauthorized();

        var result = await _commentService.CreateAsync(createDto, userGuid);

        if (result.Success)
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);

        return BadRequest(result.Message);
    }

    /// <summary>
    /// Uuendab kommentaari andmeid
    /// </summary>
    /// <param name="id">Kommentaari ID</param>
    /// <param name="updateDto">Uuendamise andmed</param>
    /// <returns>Uuendatud kommentaari andmed</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCommentDto updateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            return Unauthorized();

        var result = await _commentService.UpdateAsync(id, updateDto, userGuid);

        if (result.Success)
            return Ok(result.Data);

        return result.Message.Contains("not found") ? NotFound(result.Message) : BadRequest(result.Message);
    }

    /// <summary>
    /// Kustutab kommentaari
    /// </summary>
    /// <param name="id">Kommentaari ID</param>
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

        var result = await _commentService.DeleteAsync(id, userGuid);

        if (result.Success)
            return NoContent();

        return result.Message.Contains("not found") ? NotFound(result.Message) : BadRequest(result.Message);
    }
}