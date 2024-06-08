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


        //[HttpPost("checkout")]
        //public async Task<IActionResult> Checkout(string paymentMethod)
        //{
        //    var userId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Unauthorized(new ApiResponse<string>(false, "User not authenticated.", 401, null, new List<string> { "User not authenticated." }));
        //    }

        //    var sessionId = HttpContext.Session.Id;
        //    var response = await _userServices.CheckoutAsync(userId, sessionId, paymentMethod);
        //    if (response.Succeeded)
        //    {
        //        return Ok(response);
        //    }
        //    return BadRequest(response);
        //}


    }




}

