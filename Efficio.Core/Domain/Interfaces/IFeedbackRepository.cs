// Efficio.Core/Domain/Interfaces/IFeedbackRepository.cs
using Efficio.Core.Domain.Entities.Communication;

namespace Efficio.Core.Domain.Interfaces;

public interface IFeedbackRepository : IRepository<Feedback>
{
    Task<Feedback?> GetWithCommentsAsync(Guid feedbackId);
    Task<Feedback?> GetWithTagsAsync(Guid feedbackId);
    Task<IEnumerable<Feedback>> GetByAuthorAsync(Guid authorId);
    Task<IEnumerable<Feedback>> GetByTagAsync(Guid tagId);
}