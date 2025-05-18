using System.ComponentModel.DataAnnotations;

namespace Efficio.Core.Application.DTOs.Update;

public class UpdateTagDto
{
    [MaxLength(20)]
    public string? Title { get; set; }
    
    [MaxLength(250)]
    public string? Description { get; set; }
}