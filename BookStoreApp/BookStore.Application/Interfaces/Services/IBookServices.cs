﻿using BookStore.Application.DTOs;
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
        Task<ApiResponse<BookResponseDto>> AddBookAsync(AddBookDto bookDto);
        Task<ApiResponse<BookResponseDto>> UpdateBookAsync(UpdateBookDto bookDto);
        Task<ApiResponse<Book>> DeleteBookByIdAsync(string bookId);
        Task<BookResponseDto> GetBookByIdAsync(string bookId);
        Task<BookResponseDto> GetBookByTitleAsync(string title);
        Task<IEnumerable<BookResponseDto>> GetBooksByGenreAsync(string genreName);

        Task<IEnumerable<BookResponseDto>> GetAllBooksByPublishedDateAsync(DateTime publishedDate);
    }
}
