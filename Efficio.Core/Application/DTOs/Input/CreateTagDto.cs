using System.ComponentModel.DataAnnotations;

namespace Efficio.Core.Application.DTOs.Input;

public class CreateTagDto
{
    [Required]
    [MaxLength(20)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(250)]
    public string Description { get; set; } = string.Empty;
}