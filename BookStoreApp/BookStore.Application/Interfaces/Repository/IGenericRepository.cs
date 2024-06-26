﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Interfaces.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(string id);
        Task<List<T>> GetAllAsync();
        Task<List<T>> FindAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        void UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteAllAsync(List<T> entities);
        void SaveChangesAsync();
        Task<T> FindSingleAsync(Expression<Func<T, bool>> expression);
    }
}
