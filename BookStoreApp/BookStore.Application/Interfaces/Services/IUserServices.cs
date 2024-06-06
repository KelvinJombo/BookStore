using BookStore.Domain;
using BookStore.Domain.Entities;

namespace BookStore.Application.Interfaces.Services
{
    public interface IUserServices
    {
        Task<ApiResponse<Order>> CheckoutAsync(string userId, List<string> bookIds, string paymentMethod);
        Task<List<Order>> GetPurchaseHistoryAsync(string userId);
    }
}
