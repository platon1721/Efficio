using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Efficio.Core.Domain.Entities.Base;
using Efficio.Core.Domain.Entities.Communication;

namespace Efficio.Core.Domain.Entities.Common;

public class Department : BaseDeletableEntity
{
    [Required]
    [MaxLength(250)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public Guid HeadId { get; set; }
    public User Head { get; set; } = default!;
    
    
    public Guid? HeadDepartmentId { get; set; }
    public Department? HeadDepartment { get; set; }
    public ICollection<UserDepartment> UserDepartments { get; set; } = new List<UserDepartment>();
    public ICollection<Department> SubDepartments { get; set; } = new List<Department>();
    public ICollection<PostDepartment> PostDepartments{ get; set; } = new List<PostDepartment>();
    
    [NotMapped]
    public ICollection<Post> Posts => PostDepartments.Select(x => x.Post).ToList();
    [NotMapped]
    public ICollection<User> Users => UserDepartments.Select(x => x.User).ToList();
}