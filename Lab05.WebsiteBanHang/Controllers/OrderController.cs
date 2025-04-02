using System.Threading.Tasks;
using Lab05.WebsiteBanHang.Models;
using Lab05.WebsiteBanHang.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lab05.WebsiteBanHang.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(IOrderRepository orderRepository, UserManager<ApplicationUser> userManager)
        {
            _orderRepository = orderRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            // Nếu là Admin hoặc Employee, hiển thị tất cả đơn hàng
            if (IsAdmin || IsEmployee)
            {
                var allOrders = await _orderRepository.GetAllAsync();
                return View("AdminIndex", allOrders); // Sử dụng view tương tự OrderManagement/Index
            }

            // Nếu là Customer, chỉ hiển thị đơn hàng của họ
            var orders = await _orderRepository.GetOrdersByUserIdAsync(user.Id);
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (!IsAdmin && !IsEmployee && order.UserId != user.Id)
            {
                return AccessDenied(); // Người dùng chỉ xem được đơn hàng của mình
            }

            return View(order);
        }
    }
}