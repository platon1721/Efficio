// Efficio.API/Controllers/PostsController.cs
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
public class PostsController : BaseApiController
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    /// <summary>
    /// Tagastab kõik postitused
    /// </summary>
    /// <returns>Postituste nimekiri</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PostDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _postService.GetAllAsync();
        return Ok(result.Data);
    }

    /// <summary>
    /// Tagastab konkreetse postituse
    /// </summary>
    /// <param name="id">Postituse ID</param>
    /// <returns>Postituse andmed</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _postService.GetByIdAsync(id);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Tagastab postituse koos kommentaaridega
    /// </summary>
    /// <param name="id">Postituse ID</param>
    /// <returns>Postituse andmed koos kommentaaridega</returns>
    [HttpGet("{id}/comments")]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWithComments(Guid id)
    {
        var result = await _postService.GetWithCommentsAsync(id);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Tagastab postituse koos märksõnadega
    /// </summary>
    /// <param name="id">Postituse ID</param>
    /// <returns>Postituse andmed koos märksõnadega</returns>
    [HttpGet("{id}/tags")]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWithTags(Guid id)
    {
        var result = await _postService.GetWithTagsAsync(id);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Tagastab postituse koos osakondadega
    /// </summary>
    /// <param name="id">Postituse ID</param>
    /// <returns>Postituse andmed koos osakondadega</returns>
    [HttpGet("{id}/departments")]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWithDepartments(Guid id)
    {
        var result = await _postService.GetWithDepartmentsAsync(id);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Tagastab autori järgi postitused
    /// </summary>
    /// <param name="authorId">Autori ID</param>
    /// <returns>Postituste nimekiri</returns>
    [HttpGet("by-author/{authorId}")]
    [ProducesResponseType(typeof(IEnumerable<PostDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByAuthor(Guid authorId)
    {
        var result = await _postService.GetByAuthorAsync(authorId);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Tagastab osakonna järgi postitused
    /// </summary>
    /// <param name="departmentId">Osakonna ID</param>
    /// <returns>Postituste nimekiri</returns>
    [HttpGet("by-department/{departmentId}")]
    [ProducesResponseType(typeof(IEnumerable<PostDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByDepartment(Guid departmentId)
    {
        var result = await _postService.GetByDepartmentAsync(departmentId);

        if (result.Success)
            return Ok(result.Data);

        return NotFound(result.Message);
    }

    /// <summary>
    /// Loob uue postituse
    /// </summary>
    /// <param name="createDto">Postituse loomise andmed</param>
    /// <returns>Loodud postituse andmed</returns>
    [HttpPost]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePostDto createDto)
    {
        // Efficio.API/Controllers/PostsController.cs (jätkub)
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            return Unauthorized();

        var result = await _postService.CreateAsync(createDto, userGuid);

        if (result.Success)
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);

        return BadRequest(result.Message);
    }

    /// <summary>
    /// Uuendab postituse andmeid
    /// </summary>
    /// <param name="id">Postituse ID</param>
    /// <param name="updateDto">Uuendamise andmed</param>
    /// <returns>Uuendatud postituse andmed</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePostDto updateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            return Unauthorized();

        var result = await _postService.UpdateAsync(id, updateDto, userGuid);

        if (result.Success)
            return Ok(result.Data);

        return result.Message.Contains("not found") ? NotFound(result.Message) : BadRequest(result.Message);
    }

    /// <summary>
    /// Kustutab postituse
    /// </summary>
    /// <param name="id">Postituse ID</param>
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

        var result = await _postService.DeleteAsync(id, userGuid);

        if (result.Success)
            return NoContent();

        return result.Message.Contains("not found") ? NotFound(result.Message) : BadRequest(result.Message);
    }
}