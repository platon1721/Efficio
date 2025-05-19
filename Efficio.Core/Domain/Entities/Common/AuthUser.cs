using System.ComponentModel.DataAnnotations;

namespace Efficio.Core.Domain.Entities.Common;

public class AuthUser : User
{
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    [Required]
    public string Salt { get; set; } = string.Empty;
    
    public string? RefreshToken { get; set; }
    
    public DateTime? RefreshTokenExpiryTime { get; set; }
}