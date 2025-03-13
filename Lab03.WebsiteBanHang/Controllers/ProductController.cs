using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lab03.WebsiteBanHang.Interfaces;
using Lab03.WebsiteBanHang.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab03.WebsiteBanHang.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            if (categories == null)
            {
                Console.WriteLine("Categories is null!");
                categories = new List<Category>();
            }
            Console.WriteLine($"Categories count: {categories.Count()}");
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                // Lưu sản phẩm trước để có Id
                await _productRepository.AddAsync(product);

                // Xử lý upload nhiều hình ảnh
                if (images != null && images.Any())
                {
                    product.Images ??= new List<ProductImage>();
                    foreach (var image in images)
                    {
                        if (image != null && image.Length > 0)
                        {
                            var imagePath = await SaveImage(image);
                            product.Images.Add(new ProductImage { Url = imagePath, ProductId = product.Id });
                        }
                    }
                    await _productRepository.UpdateAsync(product); // Cập nhật để lưu danh sách ảnh
                }

                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories ?? new List<Category>(), "Id", "Name");
            return View(product);
        }

        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.GetAllAsync();
            if (categories == null)
            {
                Console.WriteLine("Categories is null in Update!");
                categories = new List<Category>();
            }
            Console.WriteLine($"Categories count in Update: {categories.Count()}");
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Product product, List<IFormFile> images)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingProduct = await _productRepository.GetByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                // Cập nhật thông tin sản phẩm
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;

                // Xử lý upload nhiều hình ảnh mới
                if (images != null && images.Any())
                {
                    existingProduct.Images ??= new List<ProductImage>();
                    foreach (var image in images)
                    {
                        if (image != null && image.Length > 0)
                        {
                            var imagePath = await SaveImage(image);
                            existingProduct.Images.Add(new ProductImage { Url = imagePath, ProductId = existingProduct.Id });
                        }
                    }
                }

                await _productRepository.UpdateAsync(existingProduct);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories ?? new List<Category>(), "Id", "Name");
            return View(product);
        }

        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            Console.WriteLine($"Product ID: {product.Id}, Images Count: {(product.Images != null ? product.Images.Count : 0)}");
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            await _productRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(image.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                throw new Exception("Định dạng file không hợp lệ. Chỉ chấp nhận .jpg, .jpeg, .png.");
            }

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Tạo tên file duy nhất để tránh ghi đè
            var fileName = Guid.NewGuid().ToString() + extension;
            var savePath = Path.Combine(folderPath, fileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + fileName;
        }
    }
}