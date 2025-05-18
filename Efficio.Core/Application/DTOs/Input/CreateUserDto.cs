using System.ComponentModel.DataAnnotations;

namespace Efficio.Core.Application.DTOs.Input;

public class CreateUserDto
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string SurName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(120)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(5)]
    public string CountryCode { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(15)]
    public string Number { get; set; } = string.Empty;
    
    [Required]
    public ICollection<Guid> DepartmentIds { get; set; } = new List<Guid>();
}