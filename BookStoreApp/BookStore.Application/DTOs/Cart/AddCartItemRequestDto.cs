using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.DTOs.Cart
{
    public class AddCartItemRequestDto
    {
        public string bookId { get; set; }
        public int Quantity { get; set; }
    }
}
