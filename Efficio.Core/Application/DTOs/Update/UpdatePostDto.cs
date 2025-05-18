namespace Efficio.Core.Application.DTOs.Update
{
    public class UpdatePostDto : UpdateFeedbackDto
    {
        public ICollection<Guid>? DepartmentIds { get; set; }
    }
}