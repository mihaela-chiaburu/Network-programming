using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;

namespace OnlineStore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relație: Category -> Products (1:M)
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Title = "Electronics" },
                new Category { Id = 2, Title = "Books" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Smartphone", Price = 499.99, CategoryId = 1 },
                new Product { Id = 2, Name = "Laptop", Price = 899.99, CategoryId = 1 },
                new Product { Id = 3, Name = "C# Programming", Price = 39.99, CategoryId = 2 },
                new Product { Id = 4, Name = "Clean Code", Price = 29.99, CategoryId = 2 }
            );
        }
    }
}
