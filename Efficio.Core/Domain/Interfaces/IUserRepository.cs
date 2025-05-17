// Efficio.Core/Domain/Interfaces/IUserRepository.cs
using Efficio.Core.Domain.Entities.Common;

namespace Efficio.Core.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserWithDepartmentsAsync(Guid id);
    Task<IEnumerable<User>> GetUsersByDepartmentAsync(Guid departmentId);
    Task<User?> GetUserByEmailAsync(string email);
}