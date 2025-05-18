namespace Efficio.Core.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string SurName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    // FullPhoneNumber or CountryCode ?
    public string PhoneNumber { get; set; } = string.Empty;
    public ICollection<DepartmentDto> Departments { get; set; } = new List<DepartmentDto>();
    
    public string FullName => $"{FirstName} {SurName}";
}