using System.ComponentModel.DataAnnotations;
using Domain.Entities.Base;
using Domain.Entities.IMS.Common;
using Domain.Entities.Interfaces;
using Domain.Entities.Common;

namespace Domain.Entities.IMS.WMS;

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