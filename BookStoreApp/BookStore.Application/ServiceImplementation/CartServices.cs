using BookStore.Application.DTOs.Cart;
using BookStore.Application.Interfaces.Repository;
using BookStore.Application.Interfaces.Services;
using BookStore.Domain;
using BookStore.Domain.Entities;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CartServices> _logger;

        public CartServices(IUnitOfWork unitOfWork, ILogger<CartServices> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

         

        public async Task<ApiResponse<string>> AddItemAsync(string bookId, int quantity)
        {
            if (string.IsNullOrWhiteSpace(bookId))
            {
                return ApiResponse<string>.Failed("Invalid bookId.", 400, new List<string> { "The bookId provided is invalid." });
            }

            if (quantity <= 0)
            {
                return ApiResponse<string>.Failed("Quantity must be greater than zero.", 400, new List<string> { "The quantity must be greater than zero." });
            }

            try
            {
                // Fetch the cart item for the session with the specified bookId
                var cartItem = await _unitOfWork.CartRepository.FindSingleAsync(c => c.BookId == bookId);

                if (cartItem != null)
                {
                    cartItem.Quantity += quantity;
                    _unitOfWork.CartRepository.UpdateAsync(cartItem);
                }
                else
                {
                    cartItem = new Cart
                    {
                        BookId = bookId,
                        Quantity = quantity
                    };
                    await _unitOfWork.CartRepository.AddAsync(cartItem);
                }

                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<string>.Success(cartItem.Id.ToString(), "Item added to cart successfully.", 200);
            }
            catch (Exception ex)
            {
                 
                 _logger.LogError(ex, "Error adding item to cart");

                return ApiResponse<string>.Failed("An error occurred while adding the item to the cart.", 500, new List<string> { ex.Message });
            }
        }







        public async Task<ApiResponse<string>> RemoveItemAsync(string cartId, string bookId)
        {
            if (string.IsNullOrWhiteSpace(cartId))
            {
                return ApiResponse<string>.Failed("Invalid cartId.", 400, new List<string> { "The cartId provided is invalid." });
            }

            if (string.IsNullOrWhiteSpace(bookId))
            {
                return ApiResponse<string>.Failed("Invalid itemId.", 400, new List<string> { "The itemId provided is invalid." });
            }

            try
            {
                // Fetch the cart item with the specified cartId and itemId
                var cartItem = await _unitOfWork.CartRepository.FindSingleAsync(c => c.Id == cartId && c.BookId == bookId);

                if (cartItem != null)
                {
                    await _unitOfWork.CartRepository.DeleteAsync(cartItem);
                    await _unitOfWork.SaveChangesAsync();
                    return ApiResponse<string>.Success(cartItem.Id.ToString(), "Item removed from cart successfully.", 200);
                }
                else
                {
                    return ApiResponse<string>.Failed("Item not found in the cart.", 404, new List<string> { "The item with the specified cartId and itemId was not found in the cart." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Deletion Process failed, Cgeck codes in the try section.");

                return ApiResponse<string>.Failed("An error occurred while removing the item from the cart.", 500, new List<string> { ex.Message });
            }
        }


        public async Task<ApiResponse<List<CartViewDto>>> ViewCartContentsAsync(string cartId)
        {
            if (string.IsNullOrWhiteSpace(cartId))
            {
                return ApiResponse<List<CartViewDto>>.Failed("Invalid cartId.", 400, new List<string> { "The cartId provided is invalid." });
            }

            try
            {
                var cartItems = await _unitOfWork.CartRepository.FindAsync(c => c.Id == cartId);

                if (cartItems != null && cartItems.Any())
                {
                    var cartViewItems = new List<CartViewDto>();

                    foreach (var cartItem in cartItems)
                    {
                        var book = await _unitOfWork.BookRepository.GetByIdAsync(cartItem.BookId);
                        if (book != null)
                        {
                            cartViewItems.Add(new CartViewDto
                            {
                                BookTitle = book.Title,
                                Quantity = cartItem.Quantity
                            });
                        }
                    }

                    return ApiResponse<List<CartViewDto>>.Success(cartViewItems, "Cart contents retrieved successfully.", 200);
                }
                else
                {
                    return ApiResponse<List<CartViewDto>>.Failed("Cart not found or empty.", 404, new List<string> { "The cart with the specified cartId was not found or is empty." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Check codes in View Cart Service class");

                return ApiResponse<List<CartViewDto>>.Failed("An error occurred while retrieving the cart contents.", 500, new List<string> { ex.Message });
            }


        }



        public async Task<ApiResponse<decimal>> GetCartTotalPriceAsync(string cartId)
        {
            if (string.IsNullOrWhiteSpace(cartId))
            {
                return ApiResponse<decimal>.Failed("Invalid cartId.", 400, new List<string> { "The cartId provided is invalid." });
            }

            try
            {
                var cartItems = await _unitOfWork.CartRepository.FindAsync(c => c.Id == cartId);

                if (cartItems != null && cartItems.Any())
                {
                    decimal totalPrice = 0;

                    foreach (var cartItem in cartItems)
                    {
                        var book = await _unitOfWork.BookRepository.GetByIdAsync(cartItem.BookId);
                        if (book != null)
                        {
                            totalPrice += book.Price * cartItem.Quantity;
                        }
                    }

                    return ApiResponse<decimal>.Success(totalPrice, "Total price of cart calculated successfully.", 200);
                }
                else
                {
                    return ApiResponse<decimal>.Failed("Cart not found or empty.", 404, new List<string> { "The cart with the specified cartId was not found or is empty." });
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Price Calculation failed, check ur logic again");
                return ApiResponse<decimal>.Failed("An error occurred while calculating the total price of the cart.", 500, new List<string> { ex.Message });
            }


        }



    }
}

