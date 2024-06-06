using BookStore.Application.Interfaces.Services;
using BookStore.Application.ServiceImplementation;
using BookStore.Domain.Entities;
using BookStore.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStore.Application.DTOs;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartServices _cartServices;

        public CartsController(ICartServices cartServices)
        {
            _cartServices = cartServices;
        }



        [HttpPost("addItem")]
        public async Task<IActionResult> AddItem([FromBody] AddCartItemRequestDto request)
        {
            try
            {
                await _cartServices.AddItemAsync(request.Book, request.Quantity);
                return Ok(new ApiResponse<string>("Item added to cart successfully."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message, 404, null, new List<string> { ex.Message }));
            }
        }

        [HttpDelete("removeItem")]
        public async Task<IActionResult> RemoveItem([FromBody] Book book)
        {
            try
            {
                await _cartServices.RemoveItemAsync(book);
                return Ok(new ApiResponse<string>("Item removed from cart successfully."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message, 404, null, new List<string> { ex.Message }));
            }
        }

        [HttpGet("totalPrice")]
        public async Task<IActionResult> GetTotalPrice()
        {
            var totalPrice = await _cartServices.GetTotalPriceAsync();
            return Ok(new ApiResponse<decimal>(totalPrice, "Total price retrieved successfully."));
        }

        [HttpGet("viewCart")]
        public async Task<IActionResult> ViewCart()
        {
            var cartItems = await _cartServices.ViewCartAsync();
            return Ok(new ApiResponse<List<CartItem>>(cartItems, "Cart items retrieved successfully."));
        }





    }
}
