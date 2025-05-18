using Efficio.Core.Domain.Entities.Enums;

namespace Efficio.Core.Application.DTOs;

public class CommentDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid MadeBy { get; set; }
    public UserDto? Author { get; set; }
    public CommentableEntityType CommentableType { get; set; }
    public Guid CommentableId { get; set; }
    public DateTime CreatedAt { get; set; }
}