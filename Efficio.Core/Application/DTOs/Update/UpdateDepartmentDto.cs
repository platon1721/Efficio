using System.ComponentModel.DataAnnotations;

namespace Efficio.Core.Application.DTOs.Update;

public class UpdateDepartmentDto
{
    [MaxLength(250)]
    public string? Name { get; set; }
    
    public Guid? HeadId { get; set; }
    
    public Guid? HeadDepartmentId { get; set; }
}