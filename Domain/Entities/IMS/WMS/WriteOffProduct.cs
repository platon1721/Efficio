using System.ComponentModel.DataAnnotations;
using Domain.Entities.Base;
using Domain.Entities.IMS.Common;

namespace Domain.Entities.IMS.WMS;

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