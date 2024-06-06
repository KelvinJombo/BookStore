using BookStore.Domain;
using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Interfaces.Services
{
    public interface IBookServices
    {
        Task<ApiResponse<Book>> AddBookAsync(Book book);
        Task<ApiResponse<Book>> UpdateBookAsync(Book book);
        Task<ApiResponse<Book>> DeleteBookByIdAsync(string bookId);
        Task<Book> GetBookByIdAsync(string bookId);
        Task<Book> GetBookByTitleAsync(string title);
        Task<IEnumerable<Book>> GetBooksByGenreAsync(int genreId);
        Task<IEnumerable<Book>> GetAllBooksByPublishedDateAsync(DateTime publishedDate);
    }
}
