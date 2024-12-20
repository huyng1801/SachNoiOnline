using Microsoft.AspNetCore.Mvc;
using SachNoiOnline.Domain.Requests;
using SachNoiOnline.Domain.ValueObjects;
using SachNoiOnline.Infrastructure.Repository;

namespace SachNoiOnline.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        // GET: api/author/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound(new { Message = "Author not found" });
            }
            return Ok(author);
        }

        // GET: api/author
        [HttpGet]
        public async Task<IActionResult> GetPaginatedAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest(new { Message = "Page number and size must be greater than zero" });
            }

            var pagination = new Pagination(pageNumber, pageSize);
            var result = await _authorRepository.GetPaginatedAsync(pagination);

            return Ok(result);
        }

        // POST: api/author
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] AuthorRequest authorRequest)
        {

            var createdAuthor = await _authorRepository.AddAsync(authorRequest);

            return Ok(createdAuthor);
        }

        // PUT: api/author/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] AuthorRequest authorRequest)
        {
            if (authorRequest == null || id != authorRequest.Id)
            {
                return BadRequest(new { Message = "Invalid author data or ID mismatch" });
            }

            var updatedAuthor = await _authorRepository.UpdateAsync(authorRequest);

            if (updatedAuthor == null)
            {
                return NotFound(new { Message = "Author not found" });
            }

            return Ok(updatedAuthor);
        }

        // DELETE: api/author/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound(new { Message = "Author not found" });
            }

            await _authorRepository.DeleteAsync(id);
            return NoContent();
        }

        // DELETE (Soft): api/author/soft-delete/{id}
        [HttpDelete("soft-delete/{id}")]
        public async Task<IActionResult> SoftDeleteAsync(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound(new { Message = "Author not found" });
            }

            await _authorRepository.SoftDeleteAsync(id);
            return NoContent();
        }
    }
}
