using BookStore.Application.Interfaces.Services;
using BookStore.Domain.Entities;
using BookStore.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStore.Application.DTOs;

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



        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDto request)
        {
            var response = await _userServices.CheckoutAsync(request.UserId, request.BookIds, request.PaymentMethod);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{userId}/purchaseHistory")]
        public async Task<IActionResult> GetPurchaseHistory(string userId)
        {
            try
            {
                var purchaseHistory = await _userServices.GetPurchaseHistoryAsync(userId);
                return Ok(new ApiResponse<List<Order>>(purchaseHistory, "Purchase history retrieved successfully."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<List<Order>>(false, ex.Message, 404, null, new List<string> { ex.Message }));
            }
        }




    }
}
