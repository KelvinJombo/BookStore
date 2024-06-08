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



        [HttpPost("add")]
        public async Task<IActionResult> AddItem(string bookId, int quantity)
        {
            var response = await _cartServices.AddItemAsync(bookId, quantity);

            if (response.Succeeded)
            {
                return Ok(response);
            }

            if (response.StatusCode == 400)
            {
                return BadRequest(response);
            }

            if (response.StatusCode == 404)
            {
                return NotFound(response);
            }

            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred.", 500, null, response.Errors));
        }





        [HttpDelete("removeItem")]
        public async Task<IActionResult> RemoveItem(string bookId)
        {
            try
            {
                await _cartServices.RemoveItemAsync(bookId);
                return Ok(new ApiResponse<string>("Item removed from cart successfully."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message, 404, null, new List<string> { ex.Message }));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message, 400, null, new List<string> { ex.Message }));
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
            var response = await _cartServices.ViewCartAsync();
            return Ok(response);
        }





    }
}
