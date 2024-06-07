using BookStore.Application.DTOs;
using BookStore.Application.Interfaces.Repository;
using BookStore.Domain.Entities;
using BookStore.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using BookStore.Application.Interfaces.Services;

namespace BookStore.Application.ServiceImplementation
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public AuthenticationServices(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, IUnitOfWork unitOfWork, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _config = config;
        }


         
        public async Task<ApiResponse<RegisterResponseDto>> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerRequestDto.Email);
            if (existingUser != null)
            {
                return ApiResponse<RegisterResponseDto>.Failed("Email already exists", 400, new List<string> { "The provided email is already in use." });
            }

            var user = new AppUser
            {
                FirstName = registerRequestDto.FirstName,
                LastName = registerRequestDto.LastName,
                Email = registerRequestDto.Email,
                PhoneNumber = registerRequestDto.PhoneNumber,
                UserName = registerRequestDto.Email,
                   
            };


            var token = "";
            var result = await _userManager.CreateAsync(user, registerRequestDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = $"https://yourapp.com/confirm-email?userId={user.Id}&token={token}";
                await _emailService.SendEmailAsync(user.Email, "Confirm your email", $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>link</a>");

                var responseDto = new RegisterResponseDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Token = token
                };

                return ApiResponse<RegisterResponseDto>.Success(responseDto, "User registered successfully. Please check your email to confirm your account.", 201);
            }
            else
            {
                return ApiResponse<RegisterResponseDto>.Failed("User registration failed", 400, result.Errors.Select(e => e.Description).ToList());
            }
        }


        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto loginDTO)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (user == null)
                {
                    return ApiResponse<LoginResponseDto>.Failed("User not found.", StatusCodes.Status404NotFound, new List<string>());
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, lockoutOnFailure: false);

                switch (result)
                {
                    case { Succeeded: true }:
                        if (!user.EmailConfirmed)
                        {
                            user.EmailConfirmed = true;
                        }

                        var role = (await _userManager.GetRolesAsync(user)).First();
                        user.LastLogin = DateTime.Now;
                        _unitOfWork.UserRepository.Update(user);
                        await _unitOfWork.SaveChangesAsync();

                        var response = new LoginResponseDto
                        {
                            JWToken = GenerateJwtToken(user, role)
                        };
                        return ApiResponse<LoginResponseDto>.Success(response, "Logged In Successfully", StatusCodes.Status200OK);

                    case { IsLockedOut: true }:
                        return ApiResponse<LoginResponseDto>.Failed($"Account is locked out. Please try again later or contact support. " +
                            $"You can unlock your account after {_userManager.Options.Lockout.DefaultLockoutTimeSpan.TotalMinutes} minutes.", StatusCodes.Status403Forbidden, new List<string>());

                    case { RequiresTwoFactor: true }:
                        return ApiResponse<LoginResponseDto>.Failed("Two-factor authentication is required.", StatusCodes.Status401Unauthorized, new List<string>());

                    case { IsNotAllowed: true }:
                        return ApiResponse<LoginResponseDto>.Failed("Login failed. Email confirmation is required.", StatusCodes.Status401Unauthorized, new List<string>());

                    default:
                        return ApiResponse<LoginResponseDto>.Failed("Login failed. Invalid email or password.", StatusCodes.Status401Unauthorized, new List<string>());
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<LoginResponseDto>.Failed("Some error occurred while logging in." + ex.Message, StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }

        private string GenerateJwtToken(AppUser contact, string roles)
        {
            var jwtSettings = _config.GetSection("JwtSettings:Secret").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, contact.Id),
                new Claim(JwtRegisteredClaimNames.Email, contact.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, contact.FirstName+" "+contact.UserName),
                new Claim(ClaimTypes.Role, roles)
            };
            var token = new JwtSecurityToken(
                issuer: _config.GetValue<string>("JwtSettings:ValidIssuer"),
                audience: _config.GetValue<string>("JwtSettings:ValidAudience"),
            //issuer: null,
            //audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_config.GetSection("JwtSettings:AccessTokenExpiration").Value)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }




    }


}
