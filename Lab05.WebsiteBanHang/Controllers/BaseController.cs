using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Lab05.WebsiteBanHang.Models;

namespace Lab05.WebsiteBanHang.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        protected bool IsAdmin => User.IsInRole(SD.Role_Admin);
        protected bool IsCompany => User.IsInRole(SD.Role_Company);
        protected bool IsCustomer => User.IsInRole(SD.Role_Customer);
        protected bool IsEmployee => User.IsInRole(SD.Role_Employee);

        protected IActionResult AccessDenied()
        {
            return View("~/Views/Shared/AccessDenied.cshtml");
        }
    }
} 