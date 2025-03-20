using System.ComponentModel.DataAnnotations;
using Domain.Entities.Interfaces;
using Efficio.Core.Domain.Entities.Base;

namespace Efficio.Core.Domain.Entities.IMS.Common;

public class ProductType : BaseDeletableEntity, IDescribable
{
    [Required]
    [MaxLength(50)]
    public string Title { get; set; } = default!;

    [Required] 
    [MaxLength(240)] 
    public string Description { get; set; } = default!;
    
    public ICollection<Product> Products { get; set; } = new List<Product>();
}