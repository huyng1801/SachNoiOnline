using Microsoft.AspNetCore.Mvc;
using SachNoiOnline.Domain.Requests;
using SachNoiOnline.Domain.ValueObjects;
using SachNoiOnline.Infrastructure.Repository;

namespace SachNoiOnline.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: api/category/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(new { Message = "Category not found" });
            }
            return Ok(category);
        }

        // GET: api/category
        [HttpGet]
        public async Task<IActionResult> GetPaginatedAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest(new { Message = "Page number and size must be greater than zero" });
            }

            var pagination = new Pagination(pageNumber, pageSize);
            var result = await _categoryRepository.GetPaginatedAsync(pagination);

            return Ok(result);
        }

        // POST: api/category
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CategoryRequest categoryRequest)
        {

            var createdCategory = await _categoryRepository.AddAsync(categoryRequest);

            return Ok(createdCategory);
        }

        // PUT: api/category/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CategoryRequest categoryRequest)
        {
            if (categoryRequest == null || id != categoryRequest.Id)
            {
                return BadRequest(new { Message = "Invalid category data or ID mismatch" });
            }

            var updatedCategory = await _categoryRepository.UpdateAsync(categoryRequest);

            if (updatedCategory == null)
            {
                return NotFound(new { Message = "Category not found" });
            }

            return Ok(updatedCategory);
        }

        // DELETE: api/category/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(new { Message = "Category not found" });
            }

            await _categoryRepository.DeleteAsync(id);
            return NoContent();
        }

        // DELETE (Soft): api/category/soft-delete/{id}
        [HttpDelete("soft-delete/{id}")]
        public async Task<IActionResult> SoftDeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(new { Message = "Category not found" });
            }

            await _categoryRepository.SoftDeleteAsync(id);
            return NoContent();
        }
    }
}
