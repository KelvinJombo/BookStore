using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Enums;

namespace BookStore.Domain.Entities
{
    public class Book : BaseEntity
    {
        [Required]
        public string Title { get; set; }         
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        public decimal Price { get; set; }   
        public Genre Genre { get; set; }
        public int Quantity { get; set; }
        public DateTime PublishedDate { get; set; }

         

    }
}
