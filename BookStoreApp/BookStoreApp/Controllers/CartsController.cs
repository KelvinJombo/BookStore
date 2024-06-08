using BookStore.Application.Interfaces.Services;
using BookStore.Application.ServiceImplementation;
using BookStore.Domain.Entities;
using BookStore.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStore.Application.DTOs;
using Microsoft.AspNetCore.Identity;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartServices _cartServices;
        private readonly UserManager<AppUser> _userManager;

        public CartsController(ICartServices cartServices, UserManager<AppUser> userManager)
        {
            _cartServices = cartServices;
            _userManager = userManager;
        }



        [HttpPost("add-item")]
        public async Task<IActionResult> AddItem(string bookId, int quantity)
        {
             
            var response = await _cartServices.AddItemAsync(bookId, quantity);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }





        [HttpDelete("{cartId}/items/")]
        public async Task<IActionResult> RemoveItem(string cartId, string bookId)
        {
            var response = await _cartServices.RemoveItemAsync(cartId, bookId);

            if (response.Succeeded)
            {
                return Ok(new { Message = response.Message, CartId = response.Data });
            }
            else
            {
                return StatusCode(response.StatusCode, new { Message = response.Message, Errors = response.Errors });
            }
        }


        [HttpGet("{cartId}/total-price")]
        public async Task<IActionResult> GetCartTotalPrice(string cartId)
        {
            var response = await _cartServices.GetCartTotalPriceAsync(cartId);

            if (response.Succeeded)
            {
                return Ok(new { Message = response.Message, TotalPrice = response.Data });
            }
            else
            {
                return StatusCode(response.StatusCode, new { Message = response.Message, Errors = response.Errors });
            }
        }


        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCartContents(string cartId)
        {
            var response = await _cartServices.ViewCartContentsAsync(cartId);

            if (response.Succeeded)
            {
                return Ok(new { Message = response.Message, CartItems = response.Data });
            }
            else
            {
                return StatusCode(response.StatusCode, new { Message = response.Message, Errors = response.Errors });
            }
        }





    }
}
