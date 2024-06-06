using BookStore.Application.Interfaces.Repository;
using BookStore.Application.Interfaces.Services;
using BookStore.Domain;
using BookStore.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.ServiceImplementation
{
    public class BookServices : IBookServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<Book>> AddBookAsync(Book book)
        {
            await _unitOfWork.BookRepository.AddAsync(book);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<Book>.Success(book, "Book added successfully.", 201);
        }


        public async Task<ApiResponse<Book>> UpdateBookAsync(Book book)
        {
            var existingBook = await _unitOfWork.BookRepository.GetByIdAsync(book.Id);
            if (existingBook != null)
            {
                _unitOfWork.BookRepository.Update(book);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<Book>.Success(book, "Book updated successfully.", 200);
            }
            else
            {
                return ApiResponse<Book>.Failed($"Book with ID {book.Id} not found.", 404, new List<string>());
            }
        }


        public async Task<ApiResponse<Book>> DeleteBookByIdAsync(string bookId)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(bookId);
            if (book != null)
            {
                await _unitOfWork.BookRepository.DeleteAsync(book);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<Book>.Success(book, "Book deleted successfully.", 200);
            }
            else
            {
                return ApiResponse<Book>.Failed($"Book with ID {bookId} not found.", 404, new List<string>());
            }
        }


        // Add the new methods below

        public async Task<Book> GetBookByIdAsync(string bookId)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(bookId);
            if (book == null)
            {
                throw new KeyNotFoundException($"Book with ID {bookId} not found.");
            }
            return book;
        }

        public async Task<Book> GetBookByTitleAsync(string title)
        {
            var book = await _unitOfWork.BookRepository.FindSingleAsync(b => b.Title == title);
            if (book == null)
            {
                throw new KeyNotFoundException($"Book with title {title} not found.");
            }
            return book;
        }

        public async Task<IEnumerable<Book>> GetBooksByGenreAsync(int genreId)
        {
            return await _unitOfWork.BookRepository.FindAsync(b => b.GenreId == genreId);
        }

        public async Task<IEnumerable<Book>> GetAllBooksByPublishedDateAsync(DateTime publishedDate)
        {
            return await _unitOfWork.BookRepository.FindAsync(b => b.PublishedDate.Date == publishedDate.Date);
        }
    }

}
