using Domain.Entities.Common;

namespace Domain.Entities.Communication;

public class Post : Feedback
{
    public ICollection<Department> Departments { get; set; } = new List<Department>();
}