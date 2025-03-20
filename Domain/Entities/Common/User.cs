using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Base;

namespace Domain.Entities.Common;

public class User : BasePersonEntity
{
    // login, password, role ...
    
    public ICollection<UserDepartment?> UserDepartments { get; set; } = new List<UserDepartment>();
    [NotMapped]
    public List<Department?> Departments => UserDepartments.Select(u => u.Department).ToList();
}