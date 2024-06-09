using AutoMapper;
using BookStore.Application.DTOs.Book;
using BookStore.Application.Interfaces.Repository;
using BookStore.Application.Interfaces.Services;
using BookStore.Domain;
using BookStore.Domain.Entities;
using BookStore.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace BookStore.Application.ServiceImplementation
{
    public class BookServices : IBookServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public BookServices(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BookServices> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }




        public async Task<ApiResponse<BookResponseDto>> AddBookAsync(AddBookDto bookDto)
        {

            try
            {
                var book = _mapper.Map<Book>(bookDto);

                await _unitOfWork.BookRepository.AddAsync(book);

                await _unitOfWork.SaveChangesAsync();

                var responseDto = _mapper.Map<BookResponseDto>(book);

                return ApiResponse<BookResponseDto>.Success(responseDto, "Book added successfully.", 201);
            }
            catch (Exception ex)
            {
                return ApiResponse<BookResponseDto>.Failed("Add Book Operation failed", 400, new List<string> { ex.Message });
                
            }
        }





        public async Task<ApiResponse<BookResponseDto>> UpdateBookAsync(UpdateBookDto bookDto)
        {
            try
            {
                 
                var existingBook = await _unitOfWork.BookRepository.GetByIdAsync(bookDto.Id);
                if (existingBook == null)
                {
                    return ApiResponse<BookResponseDto>.Failed($"Book with ID {bookDto.Id} not found.", 404, new List<string>());
                }

                 
                _mapper.Map(bookDto, existingBook);

                // Save the changes to the repository
                _unitOfWork.BookRepository.UpdateAsync(existingBook);
                await _unitOfWork.SaveChangesAsync();

                // Create a response DTO with the updated book details
                var responseDto = _mapper.Map<BookResponseDto>(existingBook);

                return ApiResponse<BookResponseDto>.Success(responseDto, "Book updated successfully.", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Update Attempt, inconclusive, check your code again");
                return ApiResponse<BookResponseDto>.Failed("An error occurred while updating the book: " + ex.Message, 500, new List<string> { ex.Message });
            }
        }





        public async Task<ApiResponse<Book>> DeleteBookByIdAsync(string bookId)
        {
            try
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
            catch (Exception)
            {

                return ApiResponse<Book>.Failed(bookId, 500, new List<string>());
            }
        }


        public async Task<ApiResponse<BookResponseDto>> GetBookByIdAsync(string bookId)
        {
            try
            {
                var book = await _unitOfWork.BookRepository.GetByIdAsync(bookId);
                if (book == null)
                {
                    throw new KeyNotFoundException($"Book with ID {bookId} not found.");
                }

                // Map the Book entity to BookResponseDto using AutoMapper
                var responseDto = _mapper.Map<BookResponseDto>(book);

                return ApiResponse<BookResponseDto>.Success(responseDto, "Book retrieved successfully.", 200);
            }
            catch (KeyNotFoundException ex)
            {
                 _logger.LogError(ex.Message, "The supplied BookId does not exist");
                return ApiResponse<BookResponseDto>.Failed($"Book with ID {bookId} not found.", 404, new List<string> { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "GetBook Service not efficient");
                return ApiResponse<BookResponseDto>.Failed("GetBook Service Failed", 400, new List<string> { ex.Message });
            }
        }




        public async Task<ApiResponse<BookResponseDto>> GetBookByTitleAsync(string title)
        {
            try
            {
                var book = await _unitOfWork.BookRepository.FindSingleAsync(b => b.Title == title);
                if (book == null)
                {
                    throw new KeyNotFoundException($"Book with title {title} not found.");
                }

                // Map the Book entity to BookResponseDto using AutoMapper
                var responseDto = _mapper.Map<BookResponseDto>(book);

                return ApiResponse<BookResponseDto>.Success(responseDto, "Process Completed Successfully", 200);
            }
            catch (Exception ex)
            {
                return ApiResponse<BookResponseDto>.Failed("Book retrieval failed. Review your code and try again.", 400, new List<string> { ex.Message });
            }
        }



        public async Task<IEnumerable<BookResponseDto>> GetBooksByGenreAsync(string genreName)
        {
            if (string.IsNullOrWhiteSpace(genreName))
            {
                throw new ArgumentException("Genre name cannot be null or empty.", nameof(genreName));
            }

            // Try to parse the genreName to the Genre enum
            if (!Enum.TryParse(typeof(Genre), genreName, true, out var genreEnum))
            {
                throw new ArgumentException($"Invalid genre name: {genreName}", nameof(genreName));
            }

            var genre = (Genre)genreEnum;

            // Find books that match the genre
            var books = await _unitOfWork.BookRepository.FindAsync(b => b.Genre == genre);

            // Map the collection of Book entities to BookResponseDto using AutoMapper
            var bookResponseDtos = _mapper.Map<IEnumerable<BookResponseDto>>(books);

            return bookResponseDtos;
        }




        public async Task<IEnumerable<BookResponseDto>> GetAllBooksByPublishedDateAsync(DateTime publishedDate)
        {
            var books = await _unitOfWork.BookRepository.FindAsync(b => b.PublishedDate.Date == publishedDate.Date);

            // Map the collection of Book entities to BookResponseDto using AutoMapper
            var bookResponseDtos = _mapper.Map<IEnumerable<BookResponseDto>>(books);

            return bookResponseDtos;
        }

    }

}
