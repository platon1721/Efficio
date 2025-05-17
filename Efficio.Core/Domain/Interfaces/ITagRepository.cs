// Efficio.Core/Domain/Interfaces/ITagRepository.cs
using Efficio.Core.Domain.Entities.Communication;

namespace Efficio.Core.Domain.Interfaces;

public interface ITagRepository : IRepository<Tag>
{
    Task<IEnumerable<Tag>> GetByFeedbackAsync(Guid feedbackId);
    Task<IEnumerable<Tag>> GetByPostAsync(Guid postId);
    Task<Tag?> GetByTitleAsync(string title);
}