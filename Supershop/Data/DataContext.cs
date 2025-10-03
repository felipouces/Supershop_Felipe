using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Supershop.Data.Entities;

namespace Supershop.Data
{
    //public class DataContext : DbContext
    // My DataContext will no longer inherit from DbContext.

    public class DataContext : IdentityDbContext<User>  // Inheriting from IdentityDbContext to support authentication and user management
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<OrderDetailsTemp> OrderDetailsTemps { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        } 
        

     }
    
}
