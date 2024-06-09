using BookStore.Application.Interfaces.Services;
using BookStore.Domain.Entities;
using BookStore.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStore.Application.DTOs;
using BookStore.Application.ServiceImplementation;
using System.Security.Claims;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UsersController(IUserServices userServices)
        {
            _userServices = userServices;
        }


        [HttpPost("{userId}/checkout/{cartId}")]
        public async Task<IActionResult> Checkout(string userId, string cartId)
        {
            var response = await _userServices.CheckoutAsync(cartId, userId);

            if (response.Succeeded)
            {
                return Ok(new { Message = response.Message, CheckoutItems = response.Data });
            }
            else
            {
                return StatusCode(response.StatusCode, new { Message = response.Message, Errors = response.Errors });
            }


        }




        [HttpGet("purchase-history/{userId}")]
        public async Task<IActionResult> GetPurchaseHistory(string userId)
        {
            try
            {
                var response = await _userServices.GetPurchaseHistoryAsync(userId);
                if (response.StatusCode == 200)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(response.StatusCode, response.Message);
                }
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


    }

}

