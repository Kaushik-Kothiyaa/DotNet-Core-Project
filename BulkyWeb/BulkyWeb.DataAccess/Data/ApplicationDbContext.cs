
using BulkyWeb.Model;
using BulkyWeb.Model.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bulky.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>     
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base .OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "C# Fundamentals",
                    Description = "An introduction to C# programming language",
                    Author = "John Doe",
                    ISBN = "1111111111111",
                    ListPrice = 99,
                    Price = 90,
                    Price50 = 85,
                    Price100 = 80,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 2,
                    Title = "ASP.NET Core for Beginners",
                    Description = "Learn how to build web apps with ASP.NET Core.",
                    Author = "Jane Smith",
                    ISBN = "2222222222222",
                    ListPrice = 120,
                    Price = 110,
                    Price50 = 100,
                    Price100 = 95,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 3,
                    Title = "Entity Framework Core In Depth",
                    Description = "Master data access with EF Core.",
                    Author = "Mike Johnson",
                    ISBN = "3333333333333",
                    ListPrice = 150,
                    Price = 140,
                    Price50 = 130,
                    Price100 = 120,
                    CategoryId = 1,
                    ImageUrl = ""
                }
            );
        }
    }
}
