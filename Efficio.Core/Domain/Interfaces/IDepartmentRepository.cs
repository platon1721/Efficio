// Efficio.Core/Domain/Interfaces/IDepartmentRepository.cs
using Efficio.Core.Domain.Entities.Common;

namespace Efficio.Core.Domain.Interfaces;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<Department?> GetWithSubDepartmentsAsync(Guid departmentId);
    Task<Department?> GetWithUsersAsync(Guid departmentId);
    Task<IEnumerable<Department>> GetByHeadIdAsync(Guid headId);
}