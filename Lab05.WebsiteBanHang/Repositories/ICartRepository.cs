using System.Threading.Tasks;
using Lab05.WebsiteBanHang.Models;

namespace Lab05.WebsiteBanHang.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserIdAsync(string userId);
        Task AddCartItemAsync(CartItem cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task RemoveCartItemAsync(int cartItemId);
        Task ClearCartAsync(string userId);
    }
}