using System.ComponentModel.DataAnnotations;
using Domain.Entities.Base;

namespace Domain.Entities.Communication;

public class PostTag : BaseEntity
{
    [Required]
    public Guid PostId { get; set; }
    public Post Post { get; set; } = default!;
    
    [Required]
    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = default!;
}