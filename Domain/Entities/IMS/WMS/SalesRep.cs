using System.ComponentModel.DataAnnotations;
using Domain.Entities.Base;

namespace Domain.Entities.IMS.WMS;

public class SalesRep : BasePersonEntity
{
    [Required]
    public Guid DistributorId { get; set; }
    public Distributor Distributor { get; set; } = default!;
}