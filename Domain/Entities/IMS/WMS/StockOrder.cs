using System.ComponentModel.DataAnnotations;
using Domain.Entities.Base;
using Domain.Entities.Enums;
using Domain.Entities.Interfaces;
using Domain.Entities.Common;

namespace Domain.Entities.IMS.WMS;

public class StockOrder: BaseDeletableEntity, IAuthorable
{
    // Status of the order.
    [Required]
    public OrderStatus Status { get; set; }
    
    // The person, that have done the order.
    [Required]
    public Guid MadeBy { get; set; } = default!;
    public User Author { get; set; } = default!;
    
    // The stock, where the order is done.
    [Required]
    public Guid StockId { get; set; } = default!;
    public Common.Stock Stock { get; set; } = default!;
    
    // The distributor, from where the order is done.
    [Required]
    public Guid DistributorId { get; set; } = default!;
    public Distributor Distributor { get; set; } = default!;
    
    
    // The person, that represents the distributor.
    [Required]
    public Guid SalesRepId { get; set; } = default!;
    public SalesRep SalesRep { get; set; } = default!;
}