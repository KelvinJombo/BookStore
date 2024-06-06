using BookStore.Application.Interfaces.Repository;
using BookStore.Domain.Entities.Helper;
using MimeKit;
using System.Net.Mail;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using BookStore.Application.Interfaces.Services;

namespace BookStore.Application.ServiceImplementation
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IUnitOfWork _unitOfWork;
        public EmailService(EmailSettings emailSettings, IUnitOfWork unitOfWork)
        {
            _emailSettings = emailSettings;
            _unitOfWork = unitOfWork;

        }



        public async Task<string> SendEmailAsync(string link, string email, string id)
        {
            try
            {
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $"<a href='{link}'>Click here to confirm your email</a>"
                };

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.Email));
                emailMessage.To.Add(new MailboxAddress(email, email));
                emailMessage.Subject = "Confirm your email";
                emailMessage.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();

                // Adjustments for SSL/TLS handshake issue
                client.ServerCertificateValidationCallback = (s, c, h, e) => true; // Permissive certificate validation
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port); // Use STARTTLS
                await client.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
                return "success";
            }
            catch (Exception ex)
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                if (user != null)
                {
                    _unitOfWork.UserRepository.DeleteAsync(user);
                    await _unitOfWork.SaveChangesAsync();
                }
                //_logger.LogError(ex, "Error occurred while sending email.");
                return "Error occurred while sending email. Check your internet connection.";
            }
        }
    }
}
