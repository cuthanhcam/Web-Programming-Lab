using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab02.WebsiteBanHang.Models;

namespace Lab02.WebsiteBanHang.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}