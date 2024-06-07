using BookStore.Application.DTOs;
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
        Task<List<CartItem>> AddItemAsync(AddBookDto bookDto, int quantity);
        Task<bool> RemoveItemAsync(AddBookDto book);
        Task<decimal> GetTotalPriceAsync();
        Task ClearAsync();
        Task<List<CartItem>> ViewCartAsync();
    }
}
