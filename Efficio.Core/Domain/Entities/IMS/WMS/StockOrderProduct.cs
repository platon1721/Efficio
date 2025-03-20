using System.ComponentModel.DataAnnotations;
using Efficio.Core.Domain.Entities.Base;
using Efficio.Core.Domain.Entities.IMS.Common;

namespace Efficio.Core.Domain.Entities.IMS.WMS;

public class StockOrderProduct : BaseEntity
{
    [Required]
    public Guid StockOrderId { get; set; } = default!;
    public StockOrder StockOrder { get; set; } = default!;
    
    [Required]
    public Guid ProductId { get; set; } = default!;
    public Product Product { get; set; } = default!;
    
    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}