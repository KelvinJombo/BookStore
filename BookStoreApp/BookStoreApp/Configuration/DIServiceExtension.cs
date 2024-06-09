using BookStore.Application.Interfaces.Repository;
using BookStore.Application.Interfaces.Services;
using BookStore.Application.ServiceImplementation;
using BookStore.Domain.Entities;
using BookStore.Domain.Entities.Helper;
using BookStore.Persistence.Context;
using BookStore.Persistence.Repository;
using BookStoreApp.Mapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

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

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                loggingBuilder.AddNLog("NLog.config");
            });


            services.AddAutoMapper(typeof(MapperProfiles));
            services.AddScoped<IBookServices, BookServices>();
            services.AddScoped<ICartServices, CartServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();

            //services.AddScoped<CartService>();


            var emailSettings = new EmailSettings();
            configuration.GetSection("EmailSettings").Bind(emailSettings);
            services.AddSingleton(emailSettings);
            services.AddTransient<IEmailService, EmailService>();



        }
    }
}
