using BookStore.Application.DTOs;
using BookStore.Domain;
using BookStore.Domain.Entities;

namespace BookStore.Application.Interfaces.Services
{
    public interface IUserServices
    {
        Task<ApiResponse<List<CheckoutDto>>> CheckoutAsync(string cartId, string userId);
        Task<ApiResponse<string>> ClearAsync(string userId);
        Task<List<Order>> GetPurchaseHistoryAsync(string userId);
    }
}
