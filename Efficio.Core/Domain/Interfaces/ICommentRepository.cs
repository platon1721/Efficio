// Efficio.Core/Domain/Interfaces/ICommentRepository.cs
using Efficio.Core.Domain.Entities.Communication;
using Efficio.Core.Domain.Entities.Enums;

namespace Efficio.Core.Domain.Interfaces;

public interface ICommentRepository : IRepository<Comment>
{
    Task<IEnumerable<Comment>> GetByAuthorAsync(Guid authorId);
    Task<IEnumerable<Comment>> GetByCommentableAsync(CommentableEntityType type, Guid commentableId);
}