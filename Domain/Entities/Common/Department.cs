using System.ComponentModel.DataAnnotations;
using Domain.Entities.Base;

namespace Domain.Entities.Common;

public class Department : BaseDeletableEntity
{
    [Required]
    [MaxLength(250)]
    public string Name { get; set; }
    
    public Guid HeadId { get; set; }
    public User Head { get; set; } = default!;
    
    public Department? HeadDepartment { get; set; }
    public ICollection<Department> SubDepartments { get; set; } = new List<Department>();
    
    
}