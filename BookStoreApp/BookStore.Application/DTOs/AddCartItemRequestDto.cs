using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.DTOs
{
    public class AddCartItemRequestDto
    {
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}
