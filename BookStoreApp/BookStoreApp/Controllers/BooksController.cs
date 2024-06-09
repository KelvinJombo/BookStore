using BookStore.Application.Interfaces.Services;
using BookStore.Domain.Entities;
using BookStore.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStore.Application.DTOs.Book;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApp.Controllers
{
    //[Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookServices _bookServices;

        public BooksController(IBookServices bookServices)
        {
            _bookServices = bookServices;
        }


        
        [HttpPost("addBook")]
        public async Task<IActionResult> AddBook([FromBody] AddBookDto book)
        {
            var response = await _bookServices.AddBookAsync(book);
            if (response.Succeeded)
            {
                // Return the response with the created book details
                return StatusCode(response.StatusCode, response);
            }
            return StatusCode(response.StatusCode, response);
        }

        //[Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateBook([FromBody] UpdateBookDto book)
        {
             

            var response = await _bookServices.UpdateBookAsync(book);
            return StatusCode(response.StatusCode, response);
        }

        //[Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBookById(string id)
        {
            var response = await _bookServices.DeleteBookByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }


        //[Authorize]
        [HttpGet("getById")]
        public async Task<IActionResult> GetBookById(string id)
        {
            try
            {
                var response = await _bookServices.GetBookByIdAsync(id);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<Book>(false, ex.Message, 404, null, new List<string> { ex.Message }));
            }
        }

        [HttpGet("getBy/{title}")]
        public async Task<IActionResult> GetBookByTitle(string title)
        {
            try
            {
                var book = await _bookServices.GetBookByTitleAsync(title);
                return Ok(book);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<Book>(false, ex.Message, 404, null, new List<string> { ex.Message }));
            }
        }

        [HttpGet("getByGenre/{genreName}")]
        public async Task<IActionResult> GetBooksByGenre(string genreName)
        {
            if (string.IsNullOrWhiteSpace(genreName))
            {
                return BadRequest("Genre name cannot be empty.");
            }

            var books = await _bookServices.GetBooksByGenreAsync(genreName);

            if (books == null || !books.Any())
            {
                return NotFound($"No books found under the genre: {genreName}");
            }

            return Ok(books);
        }


        [HttpGet("publishedDate/{publishedDate}")]
        public async Task<IActionResult> GetAllBooksByPublishedDate(DateTime publishedDate)
        {
            var books = await _bookServices.GetAllBooksByPublishedDateAsync(publishedDate);
            return Ok(books);
        }





    }
}
