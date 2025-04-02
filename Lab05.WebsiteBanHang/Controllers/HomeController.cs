using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Lab05.WebsiteBanHang.Models;
using Lab05.WebsiteBanHang.Interfaces;

namespace Lab05.WebsiteBanHang.Controllers;

public class HomeController : Controller
{
    private readonly IProductRepository _productRepository;

    public HomeController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productRepository.GetAllAsync();
        return View(products);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
