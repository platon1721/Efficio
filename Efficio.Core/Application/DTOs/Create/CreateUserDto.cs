using System.ComponentModel.DataAnnotations;

namespace Efficio.Core.Application.DTOs.Create;

public class CreateUserDto
{
    [Required(ErrorMessage = "FirstNameRequired")]
    [MaxLength(50, ErrorMessage = "FirstNameMaxLength")]
    [Display(Name = "FirstName")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "LastNameRequired")]
    [MaxLength(50, ErrorMessage = "LastNameMaxLength")]
    [Display(Name = "LastName")]
    public string SurName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "EmailRequired")]
    [EmailAddress(ErrorMessage = "EmailInvalid")]
    [MaxLength(120, ErrorMessage = "EmailMaxLength")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "CountryCodeRequired")]
    [MaxLength(5, ErrorMessage = "CountryCodeMaxLength")]
    [Display(Name = "CountryCode")]
    public string CountryCode { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "NumberRequired")]
    [MaxLength(15, ErrorMessage = "NumberMaxLength")]
    [Display(Name = "Number")]
    public string Number { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "DepartmentIdsRequired")]
    [Display(Name = "Departments")]
    public ICollection<Guid> DepartmentIds { get; set; } = new List<Guid>();
}