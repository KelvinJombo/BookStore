using BookStore.Application.Interfaces.Repository;
using BookStore.Application.Interfaces.Services;
using BookStore.Application.ServiceImplementation;
using BookStore.Domain.Entities.Helper;
using BookStore.Domain.Entities;
using BookStore.Persistence.Context;
using BookStore.Persistence.Repository;
using BookStoreApp.Mapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.Configuration
{
    public static class DIServiceExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookStoreDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("StoreConnections")));

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<BookStoreDbContext>()
                .AddDefaultTokenProviders();


            services.AddAutoMapper(typeof(MapperProfiles));
            services.AddScoped<IBookServices, BookServices>();
            services.AddScoped<ICartServices, CartServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();


             

            var emailSettings = new EmailSettings();
            configuration.GetSection("EmailSettings").Bind(emailSettings);
            services.AddSingleton(emailSettings);
            services.AddTransient<IEmailService, EmailService>();



        }
    }
}
