using BookStore.Application.Interfaces.Repository;
using BookStore.Domain.Entities;
using BookStore.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Persistence.Repository
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
         
        public BookRepository(BookStoreDbContext context) : base(context)
        {
             
        }
    }
}
