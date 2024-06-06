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

        // Checkout process
        public async Task<ApiResponse<Order>> CheckoutAsync(string userId, List<string> bookIds, string paymentMethod)
        {
            // Simulate checking if payment is successful
            bool isPaymentSuccessful = SimulatePayment(paymentMethod);

            if (isPaymentSuccessful)
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<Order>.Failed($"User with ID {userId} not found.", 404, new List<string>());
                }

                var books = new List<Book>();
                foreach (var bookId in bookIds)
                {
                    var book = await _unitOfWork.BookRepository.GetByIdAsync(bookId);
                    if (book != null)
                    {
                        books.Add(book);
                    }
                }

                if (books.Count == 0)
                {
                    return ApiResponse<Order>.Failed("No valid books found for the provided book IDs.", 400, new List<string>());
                }

                var order = new Order
                {
                    AppUser = user,
                    AppUserID = userId,
                    Books = books,
                    // Populate other necessary fields, if any
                };

                await _unitOfWork.OrderRepository.AddAsync(order);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<Order>.Success(order, "Order created successfully.", 200);
            }
            else
            {
                return ApiResponse<Order>.Failed("Payment failed. Please try again.", 400, new List<string> { "Payment processing error" });
            }
        }


        // Simulate payment processing
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
