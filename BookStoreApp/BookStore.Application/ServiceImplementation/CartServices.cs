using BookStore.Application.Interfaces.Repository;
using BookStore.Application.Interfaces.Services;
using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.ServiceImplementation
{
    public class CartServices : ICartServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Items = new List<CartItem>();
        }

        public List<CartItem> Items { get; private set; }

        public async Task AddItemAsync(Book book, int quantity)
        {
            var existingBook = await _unitOfWork.BookRepository.GetByIdAsync(book.Id);
            if (existingBook == null)
            {
                throw new KeyNotFoundException($"Book with ID {book.Id} not found.");
            }

            var existingItem = Items.FirstOrDefault(i => i.Book.Id == book.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem { Book = existingBook, Quantity = quantity });
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(Book book)
        {
            var existingBook = await _unitOfWork.BookRepository.GetByIdAsync(book.Id);
            if (existingBook == null)
            {
                throw new KeyNotFoundException($"Book with ID {book.Id} not found.");
            }

            var item = Items.FirstOrDefault(i => i.Book.Id == book.Id);
            if (item != null)
            {
                Items.Remove(item);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<decimal> GetTotalPriceAsync()
        {
            // Assuming BookRepository can fetch the latest prices for the books
            foreach (var item in Items)
            {
                var book = await _unitOfWork.BookRepository.GetByIdAsync(item.Book.Id);
                if (book != null)
                {
                    item.Book.Price = book.Price; // Update the price in the cart to the latest price
                }
            }

            return Items.Sum(i => i.Book.Price * i.Quantity);
        }

        public async Task ClearAsync()
        {
            Items.Clear();
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<CartItem>> ViewCartAsync()
        {
            // Refresh the cart items with the latest details from the database
            foreach (var item in Items)
            {
                var book = await _unitOfWork.BookRepository.GetByIdAsync(item.Book.Id);
                if (book != null)
                {
                    item.Book.Title = book.Title;
                    item.Book.Author = book.Author;
                    item.Book.Price = book.Price;
                    // Update any other relevant fields
                }
            }

            return Items;
        }
    }


}

