using BookStore.Application.Interfaces.Repository;
using BookStore.Application.Interfaces.Services;
using BookStore.Application.ServiceImplementation;
using BookStore.Domain.Entities;
using BookStore.Domain.Entities.Helper;
using BookStore.Persistence.Context;
using BookStore.Persistence.Repository;
using MailKit;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Persistence.Extensions
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



            services.AddScoped<IBookServices, BookServices>();
            services.AddScoped<ICartServices, CartServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();




            var emailSettings = new EmailSettings();
            configuration.GetSection("EmailSettings").Bind(emailSettings);
            services.AddSingleton(emailSettings);
            services.AddTransient<IEmailService, EmailService>();



        }
    }
}
