namespace Efficio.Core.Application.DTOs;

public class FeedbackDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public Guid MadeBy { get; set; }
    public UserDto? Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<TagDto> Tags { get; set; } = new List<TagDto>();
    public ICollection<CommentDto> Comments { get; set; } = new List<CommentDto>();
}