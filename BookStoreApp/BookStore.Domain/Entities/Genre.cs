using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class Genre : BaseEntity
    {
        [Required]
        [DisplayName("Genre Name")]
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }
         
    }
}
