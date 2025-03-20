using System.ComponentModel.DataAnnotations;
using Domain.Entities.Interfaces;
using Efficio.Core.Domain.Entities.Base;
using Efficio.Core.Domain.Entities.Common;
using Efficio.Core.Domain.Entities.IMS.Common;

namespace Efficio.Core.Domain.Entities.IMS.WMS;

public class StockReceipt : BaseDeletableEntity, IAuthorable
{
    // User(Worker) that add receipt to the stock system.
    [Required]
    public Guid MadeBy { get; set; }
    public User Author { get; set; } = default!;
    
    // User(Worker) that receives the delivery.
    [Required]
    public Guid ReceivedBy { get; set; }
    public User Receiver { get; set; } = default!;
    
    // Can be connected to a stock order, that was made.
    public Guid? StockOrderId { get; set; }
    public StockOrder? StockOrder { get; set; }
    
    // Can be connected to a distributor,
    public Guid? DistributorId { get; set; }
    public Distributor? Distributor { get; set; }
    
    [Required]
    public Guid StockId { get; set; }
    public Stock Stock { get; set; } = default!;

    public ICollection<StockReceiptProduct> Products { get; set; } = new List<StockReceiptProduct>();
}