using Efficio.Core.Domain.Interfaces;
using Efficio.Infrastructure.Persistence.Repositories;

namespace Efficio.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    private IUserRepository _userRepository;
    private IDepartmentRepository _departmentRepository;
    private IPostRepository _postRepository;
    private IFeedbackRepository _feedbackRepository;
    private ITagRepository _tagRepository;
    private ICommentRepository _commentRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IUserRepository Users => _userRepository ??= new UserRepository(_context);
    public IDepartmentRepository Departments => _departmentRepository ??= new DepartmentRepository(_context);
    public IPostRepository Posts => _postRepository ??= new PostRepository(_context);
    public IFeedbackRepository Feedbacks => _feedbackRepository ??= new FeedbackRepository(_context);
    public ITagRepository Tags => _tagRepository ??= new TagRepository(_context);
    public ICommentRepository Comments => _commentRepository ??= new CommentRepository(_context);

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}