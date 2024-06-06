using BookStore.Application.Interfaces.Services;
using BookStore.Domain.Entities;
using BookStore.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookServices _bookServices;

        public BooksController(IBookServices bookServices)
        {
            _bookServices = bookServices;
        }



        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            var response = await _bookServices.AddBookAsync(book);
            if (response.Succeeded)
            {
                return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, response);
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(string id, [FromBody] Book book)
        {
            if (id != book.Id)
            {
                return BadRequest("Book ID mismatch.");
            }

            var response = await _bookServices.UpdateBookAsync(book);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookById(string id)
        {
            var response = await _bookServices.DeleteBookByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(string id)
        {
            try
            {
                var book = await _bookServices.GetBookByIdAsync(id);
                return Ok(book);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<Book>(false, ex.Message, 404, null, new List<string> { ex.Message }));
            }
        }

        [HttpGet("title/{title}")]
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

        [HttpGet("genre/{genreId}")]
        public async Task<IActionResult> GetBooksByGenre(int genreId)
        {
            var books = await _bookServices.GetBooksByGenreAsync(genreId);
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
