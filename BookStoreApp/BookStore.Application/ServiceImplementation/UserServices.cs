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
    public class UserServices : IUserServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<string>> CheckoutAsync(string userId, string paymentMethod)
        {
            // Simulate payment processing
            bool paymentSuccess = SimulatePayment(paymentMethod);
            if (!paymentSuccess)
            {
                return ApiResponse<string>.Failed("Payment failed. Invalid payment method.", 400, new List<string> { "Invalid payment method." });
            }

            // Fetch all cart items for the user
            var cartItems = await _unitOfWork.CartRepository.FindAsync(c => c.AppUserID == userId);
            if (cartItems == null || !cartItems.Any())
            {
                return ApiResponse<string>.Failed("Cart is empty.", 400, new List<string> { "Cart is empty." });
            }

            // Initialize the total price and list of book IDs
            decimal totalPrice = 0;
            var allBookIDs = new List<string>();

            // Process each item in the cart
            foreach (var item in cartItems)
            {
                foreach (var bookId in item.BookIDList)
                {
                    var book = await _unitOfWork.BookRepository.GetByIdAsync(bookId);
                    if (book != null)
                    {
                        if (book.Quantity < item.Quantity)
                        {
                            return ApiResponse<string>.Failed($"Insufficient stock for book: {book.Title}", 400, new List<string> { $"Insufficient stock for book: {book.Title}" });
                        }
                        book.Quantity -= item.Quantity;
                        _unitOfWork.BookRepository.Update(book);
                        allBookIDs.Add(bookId);
                        totalPrice += book.Price * item.Quantity;
                    }
                }
            }

            // Add the order to the user's orders
            var order = new Order
            {
                AppUserID = userId,
                CreatedAt = DateTime.UtcNow,
                BookIDs = string.Join(",", allBookIDs), // Save as comma-separated string
                TotalPrice = totalPrice,
                Quantity = cartItems.Sum(i => i.Quantity)
            };
            await _unitOfWork.OrderRepository.AddAsync(order);

            // Clear the user's cart
            await _unitOfWork.CartRepository.DeleteAllAsync(cartItems);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<string>.Success("Successful", "Checkout successful.", 200);
        }









        public async Task<ApiResponse<string>> ClearAsync(string userId)
        {
            var cartItems = await _unitOfWork.CartRepository.FindAsync(c => c.AppUserID == userId);
            if (cartItems != null && cartItems.Any())
            {
                _unitOfWork.CartRepository.DeleteAllAsync(cartItems);
                await _unitOfWork.SaveChangesAsync();
                return new ApiResponse<string>(true, "Cart cleared successfully.", 200, "Cart cleared successfully.", new List<string>());
            }

            return new ApiResponse<string>(false, "Cart is already empty.", 400, null, new List<string> { "Cart is already empty." });
        }




        private bool SimulatePayment(string paymentMethod)
        {
            // Simulate payment logic based on the payment method
            switch (paymentMethod.ToLower())
            {
                case "web":
                case "ussd":
                case "transfer":
                    return true; // Simulate successful payment
                default:
                    return false; // Invalid payment method
            }
        }


        // Purchase history
        public async Task<List<Order>> GetPurchaseHistoryAsync(string userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            return await _unitOfWork.OrderRepository.FindAsync(o => o.AppUserID == userId);
        }
    }

}
