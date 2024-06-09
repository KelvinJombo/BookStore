using BookStore.Domain.Entities;
using BookStore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.DTOs.Book
{
    public class AddBookDto
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public Genre Genre { get; set; }
        public DateTime PublishedDate { get; set; }
        public int Quantity { get; set; }
    }


}
