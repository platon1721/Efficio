using System.ComponentModel.DataAnnotations;

namespace Efficio.Core.Application.DTOs.Create;

public class CreateDepartmentDto
{
    [Required]
    [MaxLength(250)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public Guid HeadId { get; set; }
    
    public Guid? HeadDepartmentId { get; set; }
}