namespace Efficio.Core.Domain.Entities.Base;

public abstract class BaseDeletableEntity : BaseEntity
{
    
    public bool IsDeleted { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
}