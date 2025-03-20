using System.Linq.Expressions;
using Efficio.Core.Domain.Entities.Base;
using Efficio.Core.Domain.Entities.Common;
using Efficio.Core.Domain.Entities.Communication;
using Efficio.Core.Domain.Entities.IMS.Common;
using Efficio.Core.Domain.Entities.IMS.WMS;
using Microsoft.EntityFrameworkCore;


namespace DAL.Data;

public class AppDbContext : DbContext
{
    #region DbSet Module
    
        #region Core Module
        
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<UserDepartment>  UserDepartments { get; set; }
        
        #endregion
        
    
        #region Communication Module
        
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<FeedbackTag> FeedbackTags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        
        #endregion
        
        #region IMS Module
    
            #region Common Module
            
            // public DbSet<Stock> Stocks { get; set; }
            // public DbSet<ProductType> ProductTypes { get; set; }
            // public DbSet<Product> Products { get; set; }
            // public DbSet<StockProduct> StockProducts { get; set; }
            
            #endregion
        
            #region WMS
            
            // public DbSet<Distributor> Distributors { get; set; }
            // public DbSet<SalesRep> SalesReps { get; set; }
            // public DbSet<StockOrder> StockOrders { get; set; }
            // public DbSet<StockOrderProduct> StockOrderProducts { get; set; }
            // public DbSet<StockReceipt> StockReceipts { get; set; }
            // public DbSet<StockReceiptProduct> StockReceiptProducts { get; set; }
            // public DbSet<WriteOff> WriteOffs { get; set; }
            // public DbSet<WriteOffProduct> WriteOffProducts { get; set; }
            
            #endregion
            
        #endregion
        
    #endregion

    #region Constructor

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    #endregion
    
    #region Model Configuration

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // User and Department relations
        modelBuilder.Entity<UserDepartment>()
            .HasKey(ud => new { ud.UserId, ud.DepartmentId });
        
        modelBuilder.Entity<UserDepartment>()
            .HasOne(ud => ud.User)
            .WithMany(u => u.UserDepartments)
            .HasForeignKey(ud => ud.UserId);
        
        modelBuilder.Entity<UserDepartment>()
            .HasOne(ud => ud.Department)
            .WithMany()
            .HasForeignKey(ud => ud.DepartmentId);
        
        // Department hierarchical relationship
        modelBuilder.Entity<Department>()
            .HasOne(d => d.HeadDepartment)
            .WithMany(d => d.SubDepartments)
            .HasForeignKey(d => d.Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Department>()
            .HasOne(d => d.Head)
            .WithMany()
            .HasForeignKey(d => d.HeadId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Communication Module
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Author)
            .WithMany()
            .HasForeignKey(c => c.MadeBy);
        
        // Tag Relationships
        modelBuilder.Entity<Tag>()
            .HasOne(t => t.Author)
            .WithMany()
            .HasForeignKey(t => t.MadeBy);
        
        // FeedbackTag relationships
        modelBuilder.Entity<FeedbackTag>()
            .HasKey(ft => new { ft.FeedbackId, ft.TagId });

        modelBuilder.Entity<FeedbackTag>()
            .HasOne(ft => ft.Feedback)
            .WithMany(f => f.FeedbackTags)
            .HasForeignKey(ft => ft.FeedbackId);

        modelBuilder.Entity<FeedbackTag>()
            .HasOne(ft => ft.Tag)
            .WithMany()
            .HasForeignKey(ft => ft.TagId);
        
        // PostTag relationships
        modelBuilder.Entity<PostTag>()
            .HasKey(pt => new { pt.PostId, pt.TagId });

        modelBuilder.Entity<PostTag>()
            .HasOne(pt => pt.Post)
            .WithMany()
            .HasForeignKey(pt => pt.PostId);

        modelBuilder.Entity<PostTag>()
            .HasOne(pt => pt.Tag)
            .WithMany()
            .HasForeignKey(pt => pt.TagId);
        
        // Post-Department relationships
        modelBuilder.Entity<PostDepartment>()
            .HasKey(pd => new { pd.PostId, pd.DepartmentId });
        
        modelBuilder.Entity<PostDepartment>()
            .HasOne(pd => pd.Post)
            .WithMany(p => p.PostDepartments)
            .HasForeignKey(pd => pd.PostId);
        
        modelBuilder.Entity<PostDepartment>()
            .HasOne(pd => pd.Department)
            .WithMany(d => d.PostDepartments)
            .HasForeignKey(pd => pd.DepartmentId);
        
        
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseDeletableEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property("DeletedBy")
                    .IsRequired(false);

                modelBuilder.Entity(entityType.ClrType)
                    .Property("DeletedAt")
                    .IsRequired(false);
                
                
                // Add global query filter for soft delete
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, "IsDeleted");
                var condition = Expression.Equal(property, Expression.Constant(false));
                var lambda = Expression.Lambda(condition, parameter);
            
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }
    
    #endregion
}