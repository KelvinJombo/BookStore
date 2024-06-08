using BookStore.Application.DTOs;
using BookStore.Application.Interfaces.Repository;
using BookStore.Application.Interfaces.Services;
using BookStore.Domain;
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
            
        }

          

        public async Task<ApiResponse<string>> AddItemAsync(string bookId, int quantity)
        {
            if (quantity <= 0)
            {
                return new ApiResponse<string>(false, "Quantity must be greater than zero.", 400, null, new List<string> { "Quantity must be greater than zero." });
            }

            var existingBook = await _unitOfWork.BookRepository.FindSingleAsync(b => b.Id == bookId);
            if (existingBook == null)
            {
                return new ApiResponse<string>(false, $"Book with ID {bookId} not found.", 404, null, new List<string> { $"Book with ID {bookId} not found." });
            }

            var existingCart = await _unitOfWork.CartRepository.FindSingleAsync(c => c.BookIDs.Contains(bookId));
            if (existingCart != null)
            {
                existingCart.Quantity += quantity;
            }
            else
            {
                var newCart = new Cart
                {
                    BookIDList = new List<string> { bookId },
                    Quantity = quantity
                };
                await _unitOfWork.CartRepository.AddAsync(newCart);
            }

            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<string>(true, "Item added successfully.", 200, "Item added successfully.", new List<string>());
        }



        public async Task<ApiResponse<string>> RemoveItemAsync(string bookId)
        {
            var existingCart = await _unitOfWork.CartRepository.FindSingleAsync(c => c.BookIDs.Contains(bookId));
            if (existingCart == null)
            {
                return new ApiResponse<string>(false, $"Cart item with Book ID {bookId} not found.", 404, null, new List<string> { $"Cart item with Book ID {bookId} not found." });
            }

            existingCart.BookIDList.Remove(bookId);
            if (!existingCart.BookIDs.Any())
            {
                _unitOfWork.CartRepository.DeleteAsync(existingCart);
            }

            await _unitOfWork.SaveChangesAsync();
            return new ApiResponse<string>(true, "Item removed successfully.", 200, "Item removed successfully.", new List<string>());
        }

        public async Task<ApiResponse<List<Cart>>> ViewCartAsync()
        {
            var cartItems = await _unitOfWork.CartRepository.GetAllAsync();
            return new ApiResponse<List<Cart>>(true, "Cart items retrieved successfully.", 200, cartItems.ToList(), new List<string>());
        }

         

        public async Task<decimal> GetTotalPriceAsync()
        {
            // Fetch all cart items from the database
            var cartItems = await _unitOfWork.CartRepository.GetAllAsync();

            foreach (var item in cartItems)
            {
                // Initialize a list to store the book details
                var books = new List<Book>();

                // Loop through each book ID in the cart item
                foreach (var bookId in item.BookIDList)
                {
                    // Fetch the book details from the repository
                    var book = await _unitOfWork.BookRepository.GetByIdAsync(bookId);
                    if (book != null)
                    {
                        // Add the book to the list
                        books.Add(book);
                    }
                }
                // Update the cart item with the fetched book details
                item.Books = books;
            }

            // Calculate the total price by summing up the price of each book multiplied by its quantity
            var totalPrice = cartItems.Sum(item => item.Books.Sum(book => book.Price) * item.Quantity);

            return totalPrice;
        }
         

    }

     
}

