using System.ComponentModel.DataAnnotations;
using Domain.Entities.Interfaces;
using Efficio.Core.Domain.Entities.Base;
using Efficio.Core.Domain.Entities.Common;
using Efficio.Core.Domain.Entities.Enums;
using Efficio.Core.Domain.Entities.IMS.Common;

namespace Efficio.Core.Domain.Entities.IMS.WMS;

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
    public Stock Stock { get; set; } = default!;
    
    // The distributor, from where the order is done.
    [Required]
    public Guid DistributorId { get; set; } = default!;
    public Distributor Distributor { get; set; } = default!;
    
    
    // The person, that represents the distributor.
    [Required]
    public Guid SalesRepId { get; set; } = default!;
    public SalesRep SalesRep { get; set; } = default!;
}