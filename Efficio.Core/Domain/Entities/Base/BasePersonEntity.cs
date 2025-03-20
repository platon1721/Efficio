using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Efficio.Core.Domain.Entities.Base;

public class BasePersonEntity : BaseDeletableEntity
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [MaxLength(50)]
    public string SurName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(120)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(5)]
    public string CountryCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(15)]
    public string Number { get; set; } = string.Empty;
    
    [NotMapped]
    public string FullPhoneNumber => $"+{CountryCode} {Number}";
}