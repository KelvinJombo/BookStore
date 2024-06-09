using AutoMapper;
using BookStore.Application.DTOs;
using BookStore.Application.DTOs.Checkout;
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
        private readonly IMapper _mapper;

        public UserServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }





        public async Task<ApiResponse<List<CheckoutDto>>> CheckoutAsync(string cartId, string userId)
        {
            if (string.IsNullOrWhiteSpace(cartId) || string.IsNullOrWhiteSpace(userId))
            {
                return ApiResponse<List<CheckoutDto>>.Failed("Invalid cartId or userId.", 400, new List<string> { "The cartId or userId provided is invalid." });
            }

            try
            {
                var cartItems = await _unitOfWork.CartRepository.FindAsync(c => c.Id == cartId);

                if (cartItems == null || !cartItems.Any())
                {
                    return ApiResponse<List<CheckoutDto>>.Failed("Cart not found or empty.", 404, new List<string> { "The cart with the specified cartId was not found or is empty." });
                }

                var checkoutItems = new List<CheckoutDto>();
                decimal totalOrderPrice = 0;
                int totalOrderQuantity = 0;
                var bookIds = new List<string>();

                foreach (var cartItem in cartItems)
                {
                    var book = await _unitOfWork.BookRepository.GetByIdAsync(cartItem.BookId);
                    if (book != null)
                    {
                        var checkoutItem = new CheckoutDto
                        {
                            Title = book.Title,
                            Price = book.Price,
                            Quantity = cartItem.Quantity,
                            TotalPrice = book.Price * cartItem.Quantity
                        };

                        checkoutItems.Add(checkoutItem);
                        totalOrderPrice += checkoutItem.TotalPrice;
                        totalOrderQuantity += cartItem.Quantity;
                        bookIds.Add(book.Id);
                    }
                }

                var order = new Order
                {
                    AppUserID = userId,
                    BookIDList = bookIds,
                    TotalPrice = totalOrderPrice,
                    Quantity = totalOrderQuantity
                };

                await _unitOfWork.OrderRepository.AddAsync(order);

                // Remove items from cart after checkout
                foreach (var cartItem in cartItems)
                {
                    await _unitOfWork.CartRepository.DeleteAsync(cartItem);
                }

                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<List<CheckoutDto>>.Success(checkoutItems, "Checkout completed successfully.", 200);
            }
            catch (Exception ex)
            {
                 

                return ApiResponse<List<CheckoutDto>>.Failed("An error occurred during checkout.", 500, new List<string> { ex.Message });
            }
        }






        public async Task<ApiResponse<string>> ClearAsync(string userId)
        {
            var cartItems = await _unitOfWork.CartRepository.FindAsync(c => c.AppUserID == userId);
            if (cartItems != null && cartItems.Any())
            {
                await _unitOfWork.CartRepository.DeleteAllAsync(cartItems);
                await _unitOfWork.SaveChangesAsync();
                return new ApiResponse<string>(true, "Cart cleared successfully.", 200, "Cart cleared successfully.", new List<string>());
            }

            return new ApiResponse<string>(false, "Cart is already empty.", 400, null, new List<string> { "Cart is already empty." });
        }




        //private bool SimulatePayment(string paymentMethod)
        //{
        //    // Simulate payment logic based on the payment method
        //    switch (paymentMethod.ToLower())
        //    {
        //        case "web":
        //        case "ussd":
        //        case "transfer":
        //            return true; // Simulate successful payment
        //        default:
        //            return false; // Invalid payment method
        //    }
        //}


        // Purchase history
        public async Task<ApiResponse<List<OrderResponseDto>>> GetPurchaseHistoryAsync(string userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            var orders = await _unitOfWork.OrderRepository.FindAsync(o => o.AppUserID == userId);
            var orderDtos = _mapper.Map<List<OrderResponseDto>>(orders);

            return ApiResponse<List<OrderResponseDto>>.Success(orderDtos, "Purchase history retrieved successfully.", 200);
        }

    }

}
