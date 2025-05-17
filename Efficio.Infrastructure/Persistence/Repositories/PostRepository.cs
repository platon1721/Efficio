using Efficio.Core.Domain.Entities.Communication;
using Efficio.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Efficio.Infrastructure.Persistence.Repositories;

public class PostRepository : Repository<Post>, IPostRepository
{
    public PostRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Post?> GetWithCommentsAsync(Guid postId)
    {
        return await Context.Posts
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == postId);
    }

    public async Task<Post?> GetWithTagsAsync(Guid postId)
    {
        var post = await Context.Posts
            .Include(p => p.FeedbackTags)
            .ThenInclude(ft => ft.Tag)
            .FirstOrDefaultAsync(p => p.Id == postId);
            
        return post;
    }

    public async Task<Post?> GetWithDepartmentsAsync(Guid postId)
    {
        return await Context.Posts
            .Include(p => p.PostDepartments)
            .ThenInclude(pd => pd.Department)
            .FirstOrDefaultAsync(p => p.Id == postId);
    }

    public async Task<IEnumerable<Post>> GetByDepartmentAsync(Guid departmentId)
    {
        return await Context.Posts
            .Where(p => p.PostDepartments.Any(pd => pd.DepartmentId == departmentId))
            .ToListAsync();
    }

    public async Task<IEnumerable<Post>> GetByAuthorAsync(Guid authorId)
    {
        return await Context.Posts
            .Where(p => p.MadeBy == authorId)
            .ToListAsync();
    }
}