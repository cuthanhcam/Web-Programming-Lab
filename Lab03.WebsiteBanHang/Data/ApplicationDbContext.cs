using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab03.WebsiteBanHang.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab03.WebsiteBanHang.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
    }
}