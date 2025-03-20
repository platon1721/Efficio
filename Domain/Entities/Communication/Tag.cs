using System.ComponentModel.DataAnnotations;
using Domain.Entities.Base;
using Domain.Entities.Interfaces;
using Domain.Entities.Common;

namespace Domain.Entities.Communication;

public class Tag : BaseDeletableEntity, IDescribable, IAuthorable
{
    [Required]
    [MaxLength(20)]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MaxLength(250)]
    public string Description { get; set; } = string.Empty;
    
    public Guid MadeBy { get; set; }
    public User Author { get; set; } = default!;
}