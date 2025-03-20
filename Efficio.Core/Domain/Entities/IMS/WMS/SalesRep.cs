using System.ComponentModel.DataAnnotations;
using Efficio.Core.Domain.Entities.Base;

namespace Efficio.Core.Domain.Entities.IMS.WMS;

public class SalesRep : BasePersonEntity
{
    [Required]
    public Guid DistributorId { get; set; }
    public Distributor Distributor { get; set; } = default!;
}