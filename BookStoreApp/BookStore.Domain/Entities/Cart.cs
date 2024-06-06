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
        [ForeignKey("CartItem")]
        public string CartItemID { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        
    }

    

     

}
