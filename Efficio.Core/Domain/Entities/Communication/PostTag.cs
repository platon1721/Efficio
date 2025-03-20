using System.ComponentModel.DataAnnotations;
using Efficio.Core.Domain.Entities.Base;

namespace Efficio.Core.Domain.Entities.Communication;

public class PostTag : BaseEntity
{
    [Required]
    public Guid PostId { get; set; }
    public Post Post { get; set; } = default!;
    
    [Required]
    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = default!;
}