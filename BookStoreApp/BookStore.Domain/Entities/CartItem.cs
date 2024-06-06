using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public Book Book { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("Cart")]
        public string CartID { get; set; }
        public Cart Cart { get; set; }
    }
}
