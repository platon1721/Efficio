// Efficio.Core/Domain/Interfaces/IPostRepository.cs
using Efficio.Core.Domain.Entities.Communication;

namespace Efficio.Core.Domain.Interfaces;

public interface IPostRepository : IRepository<Post>
{
    Task<Post?> GetWithCommentsAsync(Guid postId);
    Task<Post?> GetWithTagsAsync(Guid postId);
    Task<Post?> GetWithDepartmentsAsync(Guid postId);
    Task<IEnumerable<Post>> GetByDepartmentAsync(Guid departmentId);
    Task<IEnumerable<Post>> GetByAuthorAsync(Guid authorId);
}