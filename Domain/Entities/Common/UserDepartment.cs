using Domain.Entities.Base;

namespace Domain.Entities.Common;

public class UserDepartment: BaseDeletableEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    
    public Guid DepartmentId { get; set; }
    public Department Department { get; set; } = default!;
}