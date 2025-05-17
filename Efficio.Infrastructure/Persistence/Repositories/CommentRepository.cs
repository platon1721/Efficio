using Efficio.Core.Domain.Entities.Communication;
using Efficio.Core.Domain.Entities.Enums;
using Efficio.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Efficio.Infrastructure.Persistence.Repositories;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    public CommentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Comment>> GetByAuthorAsync(Guid authorId)
    {
        return await Context.Comments
            .Where(c => c.MadeBy == authorId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Comment>> GetByCommentableAsync(CommentableEntityType type, Guid commentableId)
    {
        return await Context.Comments
            .Where(c => c.CommentableType == type && c.CommentableId == commentableId)
            .ToListAsync();
    }
}