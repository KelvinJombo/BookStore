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



        [HttpGet("{userId}/purchase-history")]
        public async Task<IActionResult> GetPurchaseHistory(string userId)
        {
            try
            {
                var orders = await _userServices.GetPurchaseHistoryAsync(userId);
                if (orders == null || !orders.Any())
                {
                    return NotFound(new { Message = "No purchase history found for this user." });
                }
                return Ok(new { Message = "Purchase history retrieved successfully.", Orders = orders });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception (logging implementation is assumed to be in place)
                // _logger.LogError(ex, "Error retrieving purchase history");

                return StatusCode(500, new { Message = "An error occurred while retrieving the purchase history.", Errors = new List<string> { ex.Message } });
            }
        }


    }

}

