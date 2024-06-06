using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Interfaces.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IGenreRepository GenreRepository { get; }
        IBookRepository BookRepository { get; }
        IOrderRepository OrderRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
