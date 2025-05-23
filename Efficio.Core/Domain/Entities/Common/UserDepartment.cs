using System.ComponentModel.DataAnnotations;
using Efficio.Core.Domain.Entities.Base;

namespace Efficio.Core.Domain.Entities.Common;

public class UserDepartment: BaseDeletableEntity
{
    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    
    [Required]
    public Guid DepartmentId { get; set; }
    public Department Department { get; set; } = default!;
}