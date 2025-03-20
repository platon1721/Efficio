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
        
        public DbSet<Comment> Commnets { get; set; }
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
    }
    
    #endregion
}