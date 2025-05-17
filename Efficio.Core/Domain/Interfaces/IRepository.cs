// Efficio.Core/Domain/Interfaces/IRepository.cs
using System.Linq.Expressions;
using Efficio.Core.Domain.Entities.Base;

namespace Efficio.Core.Domain.Interfaces;

/// <summary>
/// Üldine liides kõikidele repositooriumidele
/// </summary>
/// <typeparam name="TEntity">Olem, mida repositoorium haldab</typeparam>
public interface IRepository<TEntity> where TEntity : BaseEntity
{
    // Päring (read)
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    
    // Lisamine (create)
    Task<TEntity> AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    
    // Uuendamine (update)
    Task UpdateAsync(TEntity entity);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities);
    
    // Kustutamine (delete)
    Task RemoveAsync(TEntity entity);
    Task RemoveRangeAsync(IEnumerable<TEntity> entities);
    
    // Kustutamine ID põhjal
    Task RemoveByIdAsync(Guid id);
    
    // Lisafunktsioonid
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);
}