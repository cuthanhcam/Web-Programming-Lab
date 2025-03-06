using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab02.WebsiteBanHang.Models;

namespace Lab02.WebsiteBanHang.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(int id);
        void Add(Category category);
        void Update(Category category);
        void Delete(int id);
    }
}