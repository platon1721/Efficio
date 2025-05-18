using System.ComponentModel.DataAnnotations;

namespace Efficio.Core.Application.DTOs.Auth;

public class RegisterDto
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
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}