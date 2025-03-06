using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab02.WebsiteBanHang.Models;

namespace Lab02.WebsiteBanHang.Repositories
{
    public class MockProductRepository : IProductRepository
    {
        private readonly List<Product> _products;
        public MockProductRepository()
        {
            _products = new List<Product>
            {
                new Product { Id = 1, Name = "Iphone 12", Price = 1000, Description = "New Iphone 12", CategoryId = 1, ImageUrl = "/images/product1.jpg" },
                new Product { Id = 2, Name = "Samsung Galaxy S21", Price = 900, Description = "New Samsung Galaxy S21", CategoryId = 1, ImageUrl = "/images/product2.jpg" },
                new Product { Id = 3, Name = "Macbook Pro 2020", Price = 2000, Description = "New Macbook Pro 2020", CategoryId = 2, ImageUrl = "/images/product3.jpg" },
                new Product { Id = 4, Name = "Dell XPS 13", Price = 1500, Description = "New Dell XPS 13", CategoryId = 2, ImageUrl = "/images/product4.jpg" },
                new Product { Id = 5, Name = "Adidas Ultraboost 21", Price = 180, Description = "New Adidas Ultraboost 21", CategoryId = 3, ImageUrl = "/images/product5.jpg" },
                new Product { Id = 6, Name = "Nike Air Force 1", Price = 120, Description = "New Nike Air Force 1", CategoryId = 3, ImageUrl = "/images/product6.jpg" }
            };
        }
        public void Add(Product product)
        {
            // product.Id = _products.Max(p => p.Id) + 1;
            // _products.Add(product);
            product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
        }

        public void Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }
        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public Product GetById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) {
                throw new Exception("Product not found");
            }
            return product;
        }

        public void Update(Product product)
        {
            // var index = _products.FindIndex(p => p.Id == product.Id);
            // if (index != -1)
            // {
            //     _products[index] = product;
            // }
            var existingProduct = GetById(product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ImageUrl = product.ImageUrl;
                existingProduct.ImageUrls = product.ImageUrls;
            }
        }
    }
}