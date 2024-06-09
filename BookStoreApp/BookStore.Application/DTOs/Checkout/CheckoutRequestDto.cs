using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.DTOs
{
    public class CheckoutRequestDto
    {
        public string UserId { get; set; }
        public string PaymentMethod { get; set; }
    }
}
