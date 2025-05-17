// Efficio.Core/Domain/Interfaces/IUnitOfWork.cs
namespace Efficio.Core.Domain.Interfaces;

/// <summary>
/// Unit of Work muster, mis võimaldab töödelda mitut repositooriumi ühe transaktsiooni raames
/// </summary>
public interface IUnitOfWork : IDisposable
{
    // Spetsiifilised repositooriumid lisatakse siia
    IUserRepository Users { get; }
    IDepartmentRepository Departments { get; }
    IPostRepository Posts { get; }
    IFeedbackRepository Feedbacks { get; }
    ITagRepository Tags { get; }
    ICommentRepository Comments { get; }
    
    // Salvestab kõik muudatused andmebaasi ühe transaktsioonina
    Task<int> CompleteAsync();
}