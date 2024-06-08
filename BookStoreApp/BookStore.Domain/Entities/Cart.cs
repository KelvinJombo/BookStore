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
        public string BookIDs { get; set; } // Comma-separated string

        [NotMapped]
        public List<string> BookIDList
        {
            get => BookIDs?.Split(',').ToList() ?? new List<string>();
            set => BookIDs = string.Join(",", value);
        }

        public List<Book> Books { get; set; } = new List<Book>();
        public string AppUserID { get; set; }
        public int Quantity { get; set; }
    }







}
