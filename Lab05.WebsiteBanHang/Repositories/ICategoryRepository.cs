using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab05.WebsiteBanHang.Models;

namespace Lab05.WebsiteBanHang.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }
}