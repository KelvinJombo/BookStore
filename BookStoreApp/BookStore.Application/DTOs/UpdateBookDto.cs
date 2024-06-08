using BookStore.Domain.Entities;
using BookStore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.DTOs
{
    public class UpdateBookDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public Genre Genre { get; set; }
        public string Author { get; set; }         
        public decimal Price { get; set; }
        public DateTime PublishedDate { get; set; }
        public string ISBN { get; set; }
         

    }

}
