using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.DTOs.Cart
{
    public class CartViewDto
    {
        public string BookTitle { get; set; }
        public int Quantity { get; set; }
    }
}
