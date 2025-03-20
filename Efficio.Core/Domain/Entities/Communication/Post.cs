using Efficio.Core.Domain.Entities.Common;

namespace Efficio.Core.Domain.Entities.Communication;

public class Post : Feedback
{
    public ICollection<Department> Departments { get; set; } = new List<Department>();
}