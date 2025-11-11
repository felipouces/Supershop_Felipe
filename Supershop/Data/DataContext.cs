using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Supershop.Data.Entities;
using System.Linq;

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


        // Habilitar a regra de apagar em cascata para evitar erros de referência circular
        // Override the OnModelCreating method to customize the model creation process
        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);


            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
        }*/

    }

}
