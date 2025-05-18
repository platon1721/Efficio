using System.ComponentModel.DataAnnotations;

namespace Efficio.Core.Application.DTOs.Update;

public class UpdateCommentDto
{
    [MaxLength(1000)]
    public string? Content { get; set; }
}