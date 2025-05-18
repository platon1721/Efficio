using System.ComponentModel.DataAnnotations;

namespace Efficio.Core.Application.DTOs.Create;

public class CreateFeedbackDto
{
    [Required]
    [MaxLength(120)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(5000)]
    public string Description { get; set; } = string.Empty;
    
    // Image will be handled by form data
    
    public ICollection<Guid> TagIds { get; set; } = new List<Guid>();
}