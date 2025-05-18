using System.ComponentModel.DataAnnotations;

namespace Efficio.Core.Application.DTOs.Update;

public class UpdateUserDto
{
    [MaxLength(50)]
    public string? FirstName { get; set; }
    
    [MaxLength(50)]
    public string? SurName { get; set; }
    
    [EmailAddress]
    [MaxLength(120)]
    public string? Email { get; set; }
    
    [MaxLength(5)]
    public string? CountryCode { get; set; }
    
    [MaxLength(15)]
    public string? Number { get; set; }
    
    public ICollection<Guid>? DepartmentIds { get; set; }
}