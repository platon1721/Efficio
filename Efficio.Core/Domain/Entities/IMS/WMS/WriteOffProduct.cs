using System.ComponentModel.DataAnnotations;
using Efficio.Core.Domain.Entities.Base;
using Efficio.Core.Domain.Entities.IMS.Common;

namespace Efficio.Core.Domain.Entities.IMS.WMS;

public class WriteOffProduct : BaseEntity
{
    [Required]
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;
    
    [Required]
    public Guid WriteOffId { get; set; }
    public WriteOff WriteOff { get; set; } = default!;
    
    [Required]
    public int Quantity { get; set; }
}