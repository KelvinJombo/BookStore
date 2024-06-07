using BookStore.Application.DTOs;
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

        public async Task<List<CartItem>> AddItemAsync(AddBookDto bookDto, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            }

            var existingBook = await _unitOfWork.BookRepository.FindSingleAsync(b => b.Title == bookDto.Title);
            if (existingBook == null)
            {
                throw new KeyNotFoundException($"Book with title '{bookDto.Title}' not found.");
            }

            var existingItem = Items.FirstOrDefault(i => i.Book.Title == bookDto.Title);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem { Book = existingBook, Quantity = quantity });
            }

            await _unitOfWork.SaveChangesAsync();
            return Items; // Return the updated cart
        }


        public async Task<bool> RemoveItemAsync(AddBookDto book)
        {
            var existingBook = await _unitOfWork.BookRepository.GetByIdAsync(book.Title);
            if (existingBook == null)
            {
                throw new KeyNotFoundException($"Book with ID {book.Title} not found.");
            }

            var item = Items.FirstOrDefault(i => i.Book.Id == book.Title);
            if (item != null)
            {
                Items.Remove(item);
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetTotalPriceAsync()
        {
             
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

