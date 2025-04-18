using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab05.WebsiteBanHang.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab05.WebsiteBanHang.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: User/Index
        public async Task<IActionResult> Index()
        {
            ViewData["Layout"] = "~/Views/Shared/_AdminLayout.cshtml";
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new List<UserRoleViewModel>();

            // Lấy thông tin người dùng hiện tại
            var currentUser = await _userManager.GetUserAsync(User);
            var isCurrentUserAdmin = await _userManager.IsInRoleAsync(currentUser, SD.Role_Admin);
            ViewBag.CurrentUserId = currentUser.Id;

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Address = user.Address,
                    Age = user.Age,
                    Roles = roles.ToList(),
                    IsCurrentUserAdmin = isCurrentUserAdmin
                });
            }

            return View(userRoles);
        }

        // GET: User/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra email đã tồn tại
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng.");
                    return View(model);
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    Address = model.Address,
                    Age = model.Age
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Đảm bảo vai trò Customer tồn tại
                    if (!await _roleManager.RoleExistsAsync(SD.Role_Customer))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
                    }

                    await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                    TempData["SuccessMessage"] = "Thêm người dùng thành công!";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // GET: User/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();

            // Kiểm tra xem user đang sửa có phải là admin không
            var isTargetUserAdmin = userRoles.Contains(SD.Role_Admin);

            // Kiểm tra xem user hiện tại có phải là admin không
            var currentUser = await _userManager.GetUserAsync(User);
            var currentUserRoles = await _userManager.GetRolesAsync(currentUser);
            var isCurrentUserAdmin = currentUserRoles.Contains(SD.Role_Admin);

            var model = new UserRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Address = user.Address,
                Age = user.Age,
                Roles = userRoles.ToList(),
                AvailableRoles = allRoles.Select(r => r.Name).ToList(),
                IsTargetUserAdmin = isTargetUserAdmin,
                IsCurrentUserAdmin = isCurrentUserAdmin
            };

            return View(model);
        }

        // POST: User/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null) return NotFound();

                // Kiểm tra trùng lặp email (trừ người dùng hiện tại)
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null && existingUser.Id != model.UserId)
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng.");
                    var availableRoles = await _roleManager.Roles.ToListAsync();
                    model.AvailableRoles = availableRoles.Select(r => r.Name).ToList();
                    return View(model);
                }

                // Kiểm tra xem user hiện tại có phải là admin không
                var currentUser = await _userManager.GetUserAsync(User);
                var currentUserRoles = await _userManager.GetRolesAsync(currentUser);
                var isCurrentUserAdmin = currentUserRoles.Contains(SD.Role_Admin);

                // Kiểm tra xem user đang sửa có phải là admin không
                var targetUserRoles = await _userManager.GetRolesAsync(user);
                var isTargetUserAdmin = targetUserRoles.Contains(SD.Role_Admin);

                // Nếu user đang sửa là admin và user hiện tại không phải admin, không cho phép sửa
                if (isTargetUserAdmin && !isCurrentUserAdmin)
                {
                    ModelState.AddModelError("", "Bạn không có quyền sửa thông tin admin.");
                    var availableRoles = await _roleManager.Roles.ToListAsync();
                    model.AvailableRoles = availableRoles.Select(r => r.Name).ToList();
                    return View(model);
                }

                // Cập nhật thông tin người dùng
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.UserName = model.Email; // Đảm bảo UserName đồng bộ với Email
                user.Address = model.Address;
                user.Age = model.Age;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                // Cập nhật vai trò - chỉ cho phép 1 role
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                
                // Chỉ thêm role đầu tiên được chọn
                if (model.Roles.Any())
                {
                    await _userManager.AddToRoleAsync(user, model.Roles.First());
                }

                TempData["SuccessMessage"] = "Cập nhật thông tin và quyền thành công!";
                return RedirectToAction(nameof(Index));
            }

            // Nếu ModelState không hợp lệ, load lại danh sách vai trò
            var allRoles = await _roleManager.Roles.ToListAsync();
            model.AvailableRoles = allRoles.Select(r => r.Name).ToList();
            return View(model);
        }

        // GET: User/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var model = new UserRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Roles = userRoles.ToList()
            };

            return View(model);
        }

        // POST: User/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Kiểm tra xem người dùng cần xóa có phải là admin không
            var isTargetUserAdmin = await _userManager.IsInRoleAsync(user, SD.Role_Admin);
            if (isTargetUserAdmin)
            {
                // Lấy thông tin người dùng hiện tại
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser.Id != user.Id) // Nếu không phải chính mình
                {
                    TempData["Error"] = "Không thể xóa tài khoản Admin khác.";
                    return RedirectToAction(nameof(Index));
                }
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Xóa người dùng thành công.";
            }
            else
            {
                TempData["Error"] = "Có lỗi xảy ra khi xóa người dùng.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}