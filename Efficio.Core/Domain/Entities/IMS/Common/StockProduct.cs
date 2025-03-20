using System.ComponentModel.DataAnnotations;
using Efficio.Core.Domain.Entities.Base;

namespace Efficio.Core.Domain.Entities.IMS.Common;

public class StockProduct : BaseDeletableEntity
{
    [Required]
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;
    
    [Required]
    public Guid StockId { get; set; }
    public Stock Stock { get; set; } = default!;
    
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}