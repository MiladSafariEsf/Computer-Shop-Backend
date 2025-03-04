using APICH.CORE.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICH.DAL
{
    public class APICH_DbContext : DbContext
    {
        public APICH_DbContext(DbContextOptions<APICH_DbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(options =>
            {
                options.ToTable(nameof(Product));
                options.HasKey(a => a.Id);
                options.HasMany(a => a.OrderDetails)
                .WithOne(a => a.Product)
                .HasForeignKey(a => a.ProductId);
                options.HasMany(x => x.Reviews)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId);
            });
            modelBuilder.Entity<User>(options =>
            {
                options.ToTable(nameof(User));
                options.HasKey(a => a.Number);
                options.HasMany(a => a.Reviews)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserNumber);
                options.HasMany(a => a.Orders)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserNumber);
            });
            modelBuilder.Entity<Orders>(options =>
            {
                options.ToTable(nameof(Orders));
                options.HasKey(a => a.Id);
                options.HasMany(a => a.OrderDetails)
                .WithOne(a => a.Order)
                .HasForeignKey(a => a.OrderId);
            });

            //v12
            modelBuilder.Entity<Categories>(options =>
            {
                options.ToTable(nameof(Categories)); 
                options.HasKey(a => a.Id);
                options.HasMany(a => a.Products)
                .WithOne(a => a.Categories)
                .HasForeignKey(a => a.CategoriesId);
            });
            modelBuilder.Entity<Reviews>(options =>
            {
                options.ToTable(nameof(Reviews));
                options.HasKey(a => a.Id);
                
            });
            modelBuilder.Entity<OrderDetails>(options =>
            {
                options.ToTable(nameof(OrderDetails));
                options.HasKey(a => a.Id);

            });
        }
    }
}
