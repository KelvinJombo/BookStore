using BookStore.Application.DTOs;
using BookStore.Domain;
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
        Task<ApiResponse<string>> AddItemAsync(string bookId, int quantity);
        Task<ApiResponse<string>> RemoveItemAsync(string bookId);
        Task<decimal> GetTotalPriceAsync();         
        Task<ApiResponse<List<Cart>>> ViewCartAsync();
         
    }
}
