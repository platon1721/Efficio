using System.ComponentModel.DataAnnotations.Schema;
using Efficio.Core.Domain.Entities.Common;

namespace Efficio.Core.Domain.Entities.Communication;

public class Post : Feedback
{

    public ICollection<PostDepartment> PostDepartments { get; set; } = new List<PostDepartment>();
    
    [NotMapped]
    public ICollection<Department> Departments => PostDepartments.Select(x => x.Department).ToList();
}