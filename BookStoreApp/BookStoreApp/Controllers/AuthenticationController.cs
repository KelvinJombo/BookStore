using BookStore.Application.DTOs.Login;
using BookStore.Application.DTOs.Register;
using BookStore.Application.Interfaces.Services;
using BookStore.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly IEmailService _emailService;

        public AuthenticationController(IAuthenticationServices authenticationServices, IEmailService emailService)
        {
            _authenticationServices = authenticationServices;
            _emailService = emailService;
        }



        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.Failed("Invalid model state.", StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
            }

            // Call registration service
            var registrationResult = await _authenticationServices.RegisterAsync(requestDto);


            if (registrationResult.Succeeded)
            {
                var data = registrationResult.Data;                 
                var confirmationLink = GenerateConfirmEmailLink(data.Id, data.Token);                 
                await _emailService.SendEmailAsync(confirmationLink, data.Email, data.Id);

                return Ok(registrationResult);
            }
            else
            {
                return BadRequest(new { Message = registrationResult.Message, Errors = registrationResult.Errors });
            }
        }



        private static string GenerateConfirmEmailLink(string id, string token)
        {
            return "https://localhost:7261/email-confirmed?UserId=" + id + "&token=" + token;
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDto loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.Failed("Invalid model state.", 400, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
            }
            return Ok(await _authenticationServices.LoginAsync(loginDTO));
        }




    }
}
