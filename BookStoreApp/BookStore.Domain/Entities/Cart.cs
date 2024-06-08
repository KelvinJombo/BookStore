using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class Cart : BaseEntity
    { 
          
        [ForeignKey("Book")]
        public string BookId { get; set; } 
        public List<Book> Books { get; set; } = new List<Book>();
        public string? AppUserID { get; set; }  // Nullable until checkout
        public int Quantity { get; set; }
         
    }

    





}
