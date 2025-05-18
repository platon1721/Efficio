namespace Efficio.Core.Application.DTOs;

public class DepartmentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public Guid HeadId { get; set; }
    public UserDto? Head { get; set; }
    
    public Guid? HeadDepartmentId { get; set; }
    public DepartmentDto? HeadDepartment { get; set; }
    
    public ICollection<DepartmentDto> SubDepartments { get; set; } = new List<DepartmentDto>();
    
    
    
}