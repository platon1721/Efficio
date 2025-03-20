using System.ComponentModel.DataAnnotations;
using Domain.Entities.Base;
using Domain.Entities.Enums;
using Domain.Entities.Interfaces;
using Domain.Entities.Common;

namespace Domain.Entities.Communication;

public class Comment: BaseDeletableEntity, IAuthorable
{
    [MaxLength(1000)]
    public string Content { get; set; }
    
    public Guid MadeBy { get; set; }
    public User Author { get; set; } = default!;
    
    public CommentableEntityType CommentableType { get; set; }
    public int CommentableId { get; set; }
}