using Bulky.Data;

using BulkyWeb.DataAccess.Repository.IRepository;
using BulkyWeb.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyWeb.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }


        public void Update(Product product)
        {
            // _db.Products.Update(product);
            var ProductFromDb = _db.Products.FirstOrDefault(p => p.Id == product.Id);
            if (ProductFromDb != null)
            {
                ProductFromDb.Title = product.Title;
                ProductFromDb.Description = product.Description;
                ProductFromDb.Category = product.Category;
                ProductFromDb.ISBN = product.ISBN;
                ProductFromDb.Author = product.Author;
                ProductFromDb.ListPrice = product.ListPrice;
                ProductFromDb.Price = product.Price;
                ProductFromDb.Price50 = product.Price50;
                ProductFromDb.Price100 = product.Price100;
                ProductFromDb.CategoryId = product.CategoryId;

                if (product.ImageUrl != null)
                {
                    ProductFromDb.ImageUrl = product.ImageUrl;
                }
            }
        }
    }
}
