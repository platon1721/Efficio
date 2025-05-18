using System.ComponentModel.DataAnnotations;
using Efficio.Core.Domain.Entities.Enums;

namespace Efficio.Core.Application.DTOs.Create;

public class CreateCommentDto
{
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;
    
    [Required]
    public CommentableEntityType CommentableType { get; set; }
    
    [Required]
    public Guid CommentableId { get; set; }
}