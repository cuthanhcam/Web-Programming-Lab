using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lab04.WebsiteBanHang.Interfaces;
using Lab04.WebsiteBanHang.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Lab04.WebsiteBanHang.Data;

namespace Lab04.WebsiteBanHang.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Company + "," + SD.Role_Employee)]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Company + "," + SD.Role_Employee)]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,CategoryId")] Product product, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                try
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
                        await _productRepository.UpdateAsync(product);
                    }

                    TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi thêm sản phẩm: " + ex.Message);
                }
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Product/Edit/5
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Company + "," + SD.Role_Employee)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Company + "," + SD.Role_Employee)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,CategoryId")] Product product, List<IFormFile> images, List<int> deleteImages)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
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

                    // Xử lý xóa ảnh được chọn
                    if (deleteImages != null && deleteImages.Any())
                    {
                        existingProduct.Images ??= new List<ProductImage>();
                        var imagesToDelete = existingProduct.Images.Where(img => deleteImages.Contains(img.Id)).ToList();
                        foreach (var image in imagesToDelete)
                        {
                            // Xóa file ảnh từ thư mục
                            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.Url.TrimStart('/'));
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                            existingProduct.Images.Remove(image);
                        }
                    }

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
                    TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Product/Delete/5
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Company + "," + SD.Role_Employee)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Company + "," + SD.Role_Employee)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ProductExists(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product != null;
        }

        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Company)]
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            if (categories == null)
            {
                categories = new List<Category>();
            }
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Company)]
        public async Task<IActionResult> Add(Product product, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                try
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

                    TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi thêm sản phẩm: " + ex.Message);
                }
            }

            // Nếu có lỗi, load lại danh sách categories
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories ?? new List<Category>(), "Id", "Name");
            return View(product);
        }

        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Company)]
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
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Company)]
        public async Task<IActionResult> Update(int id, Product product, List<IFormFile> images, List<int> deleteImages)
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

                // Xử lý xóa ảnh được chọn
                if (deleteImages != null && deleteImages.Any())
                {
                    existingProduct.Images ??= new List<ProductImage>();
                    var imagesToDelete = existingProduct.Images.Where(img => deleteImages.Contains(img.Id)).ToList();
                    foreach (var image in imagesToDelete)
                    {
                        // Xóa file ảnh từ thư mục
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.Url.TrimStart('/'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        existingProduct.Images.Remove(image);
                    }
                }

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
                TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
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