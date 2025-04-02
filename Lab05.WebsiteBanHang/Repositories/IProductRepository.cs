using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab05.WebsiteBanHang.Models;

namespace Lab05.WebsiteBanHang.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<List<Product>> GetFilteredProductsAsync(string searchString, int? categoryId, string sortOrder);
    }
}