using BookStore.Application.Interfaces.Repository;
using BookStore.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStore.Persistence.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class 
    {
        private readonly BookStoreDbContext _context;

        public GenericRepository(BookStoreDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void DeleteAllAsync(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<T> FindSingleAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }


        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async void SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
