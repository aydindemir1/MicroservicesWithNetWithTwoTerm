
using Microsoft.EntityFrameworkCore;
using Order.Domain.Read;
using Order.Domain.Write;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Repository.SqlServer.Write
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Order.Domain.Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<Product>().Property(p => p.Id).ValueGeneratedNever();

            base.OnModelCreating(modelBuilder);

        }
    }
}
