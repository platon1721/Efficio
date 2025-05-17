using Efficio.Core.Domain.Entities.Communication;
using Efficio.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Efficio.Infrastructure.Persistence.Repositories;

public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
{
    public FeedbackRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Feedback?> GetWithCommentsAsync(Guid feedbackId)
    {
        return await Context.Feedbacks
            .Include(f => f.Comments)
            .FirstOrDefaultAsync(f => f.Id == feedbackId);
    }

    public async Task<Feedback?> GetWithTagsAsync(Guid feedbackId)
    {
        return await Context.Feedbacks
            .Include(f => f.FeedbackTags)
            .ThenInclude(ft => ft.Tag)
            .FirstOrDefaultAsync(f => f.Id == feedbackId);
    }

    public async Task<IEnumerable<Feedback>> GetByAuthorAsync(Guid authorId)
    {
        return await Context.Feedbacks
            .Where(f => f.MadeBy == authorId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Feedback>> GetByTagAsync(Guid tagId)
    {
        return await Context.Feedbacks
            .Where(f => f.FeedbackTags.Any(ft => ft.TagId == tagId))
            .ToListAsync();
    }
}