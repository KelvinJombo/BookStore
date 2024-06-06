using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Interfaces.Repository
{
    public interface IGenreRepository : IGenericRepository<Genre>
    {
        
    }
}
