using System.ComponentModel.DataAnnotations;

namespace Efficio.Core.Application.DTOs.Create;

public class CreatePostDto : CreateFeedbackDto
{
    [Required]
    public ICollection<Guid> DepartmentIds { get; set; } = new List<Guid>();
}