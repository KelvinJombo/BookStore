using BookStore.Application.Interfaces.Services;
using BookStore.Domain.Entities;
using BookStore.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStore.Application.DTOs;
using BookStore.Application.ServiceImplementation;

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
        public async Task<IActionResult> Checkout(string userId, string paymentMethod)
        {
            var result = await _userServices.CheckoutAsync(userId, paymentMethod);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }


    }




}

