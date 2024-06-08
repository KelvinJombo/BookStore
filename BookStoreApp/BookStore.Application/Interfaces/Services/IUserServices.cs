using BookStore.Domain;
using BookStore.Domain.Entities;

namespace BookStore.Application.Interfaces.Services
{
    public interface IUserServices
    {
        Task<ApiResponse<string>> CheckoutAsync(string userId, string paymentMethod);
        Task<ApiResponse<string>> ClearAsync(string userId);
        Task<List<Order>> GetPurchaseHistoryAsync(string userId);
    }
}
