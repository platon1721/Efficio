using System.ComponentModel.DataAnnotations;
using Domain.Entities.Interfaces;
using Efficio.Core.Domain.Entities.Base;
using Efficio.Core.Domain.Entities.Common;
using Efficio.Core.Domain.Entities.IMS.Common;

namespace Efficio.Core.Domain.Entities.IMS.WMS;

public class WriteOff : BaseDeletableEntity, IAuthorable
{
    // The User that made this write-off
    [Required]
    public Guid MadeBy { get; set; } = default!;
    public User Author { get; set; } = default!;


    // The Stock where the write-off was done
    [Required]
    public Guid StockId { get; set; } = default!;
    public Stock Stock { get; set; } = default!;
    
    [Required]
    [MaxLength(500)]
    public string Reason { get; set; } = default!;
}