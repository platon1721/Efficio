using System.ComponentModel.DataAnnotations;

namespace Efficio.Core.Application.DTOs.Update;

public class UpdateFeedbackDto
{
    [MaxLength(120)]
    public string? Title { get; set; }
    
    [MaxLength(5000)]
    public string? Description { get; set; }
    
    // Image handling will be done separately through form data
    
    public ICollection<Guid>? TagIds { get; set; }
}