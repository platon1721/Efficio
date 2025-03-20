using Domain.Entities.Common;

namespace Domain.Entities.Interfaces;

public interface IAuthorable
{
    // User id, that initialised given object
    Guid MadeBy { get; set; }
    
    User Author { get; set; }
    
}