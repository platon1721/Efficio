using Efficio.Core.Domain.Entities.Common;
using Efficio.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Efficio.Infrastructure.Persistence.Repositories;

public class DepartmentRepository : Repository<Department>, IDepartmentRepository
{
    public DepartmentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Department?> GetWithSubDepartmentsAsync(Guid departmentId)
    {
        return await Context.Departments
            .Include(d => d.SubDepartments)
            .FirstOrDefaultAsync(d => d.Id == departmentId);
    }

    public async Task<Department?> GetWithUsersAsync(Guid departmentId)
    {
        return await Context.Departments
            .Include(d => d.UserDepartments)
            .ThenInclude(ud => ud.User)
            .FirstOrDefaultAsync(d => d.Id == departmentId);
    }

    public async Task<IEnumerable<Department>> GetByHeadIdAsync(Guid headId)
    {
        return await Context.Departments
            .Where(d => d.HeadId == headId)
            .ToListAsync();
    }
}