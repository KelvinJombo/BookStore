using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task<string> SendEmailAsync(string link, string email, string id);
    }
}
