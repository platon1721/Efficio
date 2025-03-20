using System.ComponentModel.DataAnnotations;
using Domain.Entities.Interfaces;
using Efficio.Core.Domain.Entities.Base;
using Efficio.Core.Domain.Entities.Common;

namespace Efficio.Core.Domain.Entities.Communication;

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