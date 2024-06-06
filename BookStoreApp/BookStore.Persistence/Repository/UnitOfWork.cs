using BookStore.Application.Interfaces.Repository;
using BookStore.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookStoreDbContext _context;
        public UnitOfWork(BookStoreDbContext context)
        {
            _context = context;
            UserRepository = new UserRepository(context);
            GenreRepository = new GenreRepository(context);
            BookRepository = new BookRepository(context);
            CartRepository = new CartRepository(context);
            OrderRepository = new OrderRepository(context);
        }


        public IUserRepository UserRepository { get; set; }
        public IGenreRepository GenreRepository { get; set; }
        public IBookRepository BookRepository { get; set; }
        public ICartRepository CartRepository { get; set; }
        public IOrderRepository OrderRepository { get; set; }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }


    }
}
