using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class Order : BaseEntity
    {
        public AppUser AppUser { get; set; }
        [ForeignKey("AppUser")]
        public string AppUserID { get; set; }
        public ICollection<Book> Books { get; set; }
       
    }

}
