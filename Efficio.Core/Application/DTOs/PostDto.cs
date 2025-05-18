namespace Efficio.Core.Application.DTOs;

public class PostDto : FeedbackDto
{
    public ICollection<DepartmentDto> Departments { get; set; } = new List<DepartmentDto>();
}