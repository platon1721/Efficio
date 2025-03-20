using System.ComponentModel.DataAnnotations;
using Domain.Entities.Interfaces;
using Efficio.Core.Domain.Entities.Base;
using Efficio.Core.Domain.Entities.Common;
using Efficio.Core.Domain.Entities.Enums;

namespace Efficio.Core.Domain.Entities.Communication;

public class Comment: BaseDeletableEntity, IAuthorable
{
    [MaxLength(1000)]
    public string Content { get; set; }
    
    public Guid MadeBy { get; set; }
    public User Author { get; set; } = default!;
    
    public CommentableEntityType CommentableType { get; set; }
    public int CommentableId { get; set; }
}