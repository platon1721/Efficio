using Domain.Entities.Communication;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

public class CommunicationDbContext : AppDbContext
{
    public DbSet<Comment> Commnets { get; set; } = default!;
    public DbSet<Feedback> Feedbacks { get; set; } = default!;
    public DbSet<Tag> Tags { get; set; } = default!;
    public DbSet<Post> Posts { get; set; } = default!;
    public DbSet<FeedbackTag> FeedbackTags { get; set; } = default!;
    public DbSet<PostTag> PostTags { get; set; } = default!;
    
    
    public CommunicationDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    { }
}