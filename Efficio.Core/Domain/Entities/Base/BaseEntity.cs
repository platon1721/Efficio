namespace Efficio.Core.Domain.Entities.Base;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}