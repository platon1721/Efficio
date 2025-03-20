using System.ComponentModel.DataAnnotations;
using Domain.Entities.Base;
using Domain.Entities.Interfaces;

namespace Domain.Entities.IMS.Common;

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