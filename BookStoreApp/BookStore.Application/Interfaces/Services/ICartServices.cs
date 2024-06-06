using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Interfaces.Services
{
    public interface ICartServices
    {
        Task AddItemAsync(Book book, int quantity);
        Task RemoveItemAsync(Book book);
        Task<decimal> GetTotalPriceAsync();
        Task ClearAsync();
        Task<List<CartItem>> ViewCartAsync();
    }
}
