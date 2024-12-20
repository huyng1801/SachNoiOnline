using Microsoft.AspNetCore.Mvc;
using SachNoiOnline.Domain.Requests;
using SachNoiOnline.Domain.ValueObjects;
using SachNoiOnline.Infrastructure.Repository;
using System.Threading.Tasks;

namespace SachNoiOnline.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NarratorController : ControllerBase
    {
        private readonly INarratorRepository _narratorRepository;

        public NarratorController(INarratorRepository narratorRepository)
        {
            _narratorRepository = narratorRepository;
        }

        // GET: api/narrator/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var narrator = await _narratorRepository.GetByIdAsync(id);
            if (narrator == null)
            {
                return NotFound(new { Message = "Narrator not found" });
            }
            return Ok(narrator);
        }

        // GET: api/narrator
        [HttpGet]
        public async Task<IActionResult> GetPaginatedAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest(new { Message = "Page number and size must be greater than zero" });
            }

            var pagination = new Pagination(pageNumber, pageSize);
            var result = await _narratorRepository.GetPaginatedAsync(pagination);

            return Ok(result);
        }

        // POST: api/narrator
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] NarratorRequest narratorRequest)
        {

            var createdNarrator = await _narratorRepository.AddAsync(narratorRequest);

            return Ok(createdNarrator);
        }

        // PUT: api/narrator/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] NarratorRequest narratorRequest)
        {
            if (narratorRequest == null || id != narratorRequest.Id)
            {
                return BadRequest(new { Message = "Invalid narrator data or ID mismatch" });
            }

            var updatedNarrator = await _narratorRepository.UpdateAsync(narratorRequest);

            if (updatedNarrator == null)
            {
                return NotFound(new { Message = "Narrator not found" });
            }

            return Ok(updatedNarrator);
        }

        // DELETE: api/narrator/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var narrator = await _narratorRepository.GetByIdAsync(id);
            if (narrator == null)
            {
                return NotFound(new { Message = "Narrator not found" });
            }

            await _narratorRepository.DeleteAsync(id);
            return NoContent();
        }

        // DELETE (Soft): api/narrator/soft-delete/{id}
        [HttpDelete("soft-delete/{id}")]
        public async Task<IActionResult> SoftDeleteAsync(int id)
        {
            var narrator = await _narratorRepository.GetByIdAsync(id);
            if (narrator == null)
            {
                return NotFound(new { Message = "Narrator not found" });
            }

            await _narratorRepository.SoftDeleteAsync(id);
            return NoContent();
        }
    }
}
