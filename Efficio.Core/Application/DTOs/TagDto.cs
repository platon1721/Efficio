namespace Efficio.Core.Application.DTOs;

public class TagDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid MadeBy { get; set; }
    public UserDto? Author { get; set; }
}