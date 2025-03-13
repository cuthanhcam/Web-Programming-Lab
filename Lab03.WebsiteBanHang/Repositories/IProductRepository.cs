using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab03.WebsiteBanHang.Models;

namespace Lab03.WebsiteBanHang.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }
}