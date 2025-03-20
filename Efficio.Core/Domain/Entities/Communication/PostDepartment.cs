using System.ComponentModel.DataAnnotations;
using Efficio.Core.Domain.Entities.Base;
using Efficio.Core.Domain.Entities.Common;

namespace Efficio.Core.Domain.Entities.Communication;

public class PostDepartment : BaseEntity
{
    [Required]
    public Guid PostId { get; set; }
    public Post Post { get; set; } = default!;
    
    [Required]
    public Guid DepartmentId { get; set; }
    public Department Department { get; set; } = default!;
}