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
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        //private readonly BookStoreDbContext _context;
        public GenreRepository(BookStoreDbContext context) : base(context) 
        {
                //_context = context;
        }
    }
    
}
