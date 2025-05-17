using Efficio.Core.Domain.Entities.Communication;
using Efficio.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Efficio.Infrastructure.Persistence.Repositories;

public class TagRepository : Repository<Tag>, ITagRepository
{
    public TagRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Tag>> GetByFeedbackAsync(Guid feedbackId)
    {
        return await Context.FeedbackTags
            .Where(ft => ft.FeedbackId == feedbackId)
            .Select(ft => ft.Tag)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tag>> GetByPostAsync(Guid postId)
    {
        return await Context.PostTags
            .Where(pt => pt.PostId == postId)
            .Select(pt => pt.Tag)
            .ToListAsync();
    }

    public async Task<Tag?> GetByTitleAsync(string title)
    {
        return await Context.Tags
            .FirstOrDefaultAsync(t => t.Title == title);
    }
}