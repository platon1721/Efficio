using System.ComponentModel.DataAnnotations;
using Efficio.Core.Domain.Entities.Base;

namespace Efficio.Core.Domain.Entities.IMS.WMS;

public class Distributor : BaseDeletableEntity
{
    [Required]
    [MaxLength(60)]
    public string Name { get; set; } = default!;
    
    public ICollection<SalesRep> SalesReps { get; set; } = new List<SalesRep>();
}