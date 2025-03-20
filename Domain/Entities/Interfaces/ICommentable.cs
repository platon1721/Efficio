using Domain.Entities.Communication;

namespace Domain.Entities.Interfaces;

public interface ICommentable
{
    ICollection<Comment> Comments { get; set; }
}