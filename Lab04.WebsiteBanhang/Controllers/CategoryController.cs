using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab04.WebsiteBanHang.Interfaces;
using Lab04.WebsiteBanHang.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Lab04.WebsiteBanHang.Data;

namespace Lab04.WebsiteBanHang.Controllers
{
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Company)]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return View(categories);
        }

        public async Task<IActionResult> Add()
        {
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Category model)
        {
            if (ModelState.IsValid)
            {
                model.Products ??= new List<Product>();
                await _categoryRepository.AddAsync(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Category model)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.UpdateAsync(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _categoryRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}