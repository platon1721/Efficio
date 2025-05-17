using Efficio.Core.Domain.Entities.Common;
using Efficio.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Efficio.Infrastructure.Persistence.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetUserWithDepartmentsAsync(Guid id)
    {
        return await Context.Users
            .Include(u => u.UserDepartments)
            .ThenInclude(ud => ud.Department)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> GetUsersByDepartmentAsync(Guid departmentId)
    {
        return await Context.Users
            .Where(u => u.UserDepartments.Any(ud => ud.DepartmentId == departmentId))
            .ToListAsync();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await Context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}