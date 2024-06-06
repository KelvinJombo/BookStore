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
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(BookStoreDbContext context) : base(context)
        {
                
        }
    }
}
