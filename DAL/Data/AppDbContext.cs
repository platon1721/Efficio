using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Department> Departments { get; set; } = default!;
    public DbSet<UserDepartment>  UserDepartments { get; set; } = default!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}