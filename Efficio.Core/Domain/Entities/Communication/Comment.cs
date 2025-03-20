using System.ComponentModel.DataAnnotations;
using Domain.Entities.Interfaces;
using Efficio.Core.Domain.Entities.Base;
using Efficio.Core.Domain.Entities.Common;
using Efficio.Core.Domain.Entities.Enums;

namespace Efficio.Core.Domain.Entities.Communication;

public class Comment: BaseDeletableEntity, IAuthorable
{
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; } = default!;
    
    [Required]
    public Guid MadeBy { get; set; }
    public User Author { get; set; } = default!;
    
    [Required]
    public CommentableEntityType CommentableType { get; set; }
    [Required]
    public Guid CommentableId { get; set; }
}