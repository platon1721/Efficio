using System.ComponentModel.DataAnnotations;
using Domain.Entities.Interfaces;
using Efficio.Core.Domain.Entities.Base;
using Efficio.Core.Domain.Entities.Enums;
using Efficio.Core.Domain.Entities.IMS.WMS;

namespace Efficio.Core.Domain.Entities.IMS.Common;

public class Product: BaseDeletableEntity, IDescribable
{
    [Required]
    [MaxLength(60)]
    public string Title { get; set; } = default!;
    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = default!;
    
    // Distributor from where stock can order that product.
    // If that is product of product, then there is no distributor.
    public Guid? DistributorId { get; set; }
    public Distributor? Distributor { get; set; }
    
    
    // Picture
    [MaxLength(255)]
    public string? LargeImagePath { get; set; }
    
    [MaxLength(255)]
    public string? ButtonImagePath { get; set; }

    [Range(0, 5 * 1024 * 1024, ErrorMessage = "Picture size must be between 0 and 5MB.")]
    public long? ImageSize { get; set; }
    
    [Required]
    public BaseUnitType BaseUnit { get; set; }
}