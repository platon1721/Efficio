using System.ComponentModel.DataAnnotations;
using Domain.Entities.Base;
using Domain.Entities.IMS.Common;

namespace Domain.Entities.IMS.WMS;

public class StockReceiptProduct : BaseEntity
{
    [Required]
    public Guid StockReceiptId { get; set; } = default!;
    public StockReceipt StockReceipt { get; set; } = default!;
    
    [Required]
    public Guid ProductId { get; set; } = default!;
    public Product Product { get; set; } = default!;
    
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}