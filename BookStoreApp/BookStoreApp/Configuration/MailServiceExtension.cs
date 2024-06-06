using BookStore.Domain.Entities.Helper;

namespace BookStoreApp.Configuration
{
    public static class MailServiceExtension
    {
        public static void AddMailService(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
        }
    }
}
