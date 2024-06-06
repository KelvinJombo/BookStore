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
    public class UserRepository : GenericRepository<AppUser>, IUserRepository
    {
         
        public UserRepository(BookStoreDbContext context) : base(context)
        {
             
        }
    }
}
