using System.ComponentModel.DataAnnotations;
using Domain.Entities.Base;
using Domain.Entities.Common;

namespace Domain.Entities.IMS.Common;

public class DepartmentStock : BaseDeletableEntity
{
    [Required]
    public Guid DepartmentId { get; set; }
    public Department Department { get; set; } = default!;
    
    [Required]
    public Guid StockId { get; set; }
    public Stock Stock { get; set; } = default!;
}