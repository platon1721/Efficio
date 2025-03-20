using System.ComponentModel.DataAnnotations.Schema;
using Efficio.Core.Domain.Entities.Base;

namespace Efficio.Core.Domain.Entities.Common;

public class User : BasePersonEntity
{
    // login, password, role ...
    
    public ICollection<UserDepartment> UserDepartments { get; set; } = new List<UserDepartment>();
    
    [NotMapped]
    public List<Department> Departments => UserDepartments.Select(u => u.Department).ToList();
}