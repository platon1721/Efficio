using System.ComponentModel.DataAnnotations;
using Efficio.Core.Domain.Entities.Base;
using Efficio.Core.Domain.Entities.Common;

namespace Efficio.Core.Domain.Entities.IMS.Common;

public class DepartmentStock : BaseDeletableEntity
{
    [Required]
    public Guid DepartmentId { get; set; }
    public Department Department { get; set; } = default!;
    
    [Required]
    public Guid StockId { get; set; }
    public Stock Stock { get; set; } = default!;
}