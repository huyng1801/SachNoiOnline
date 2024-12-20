using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SachNoiOnline.Domain.Filters;
using SachNoiOnline.Domain.Requests;
using SachNoiOnline.Domain.Responses;
using SachNoiOnline.Domain.ValueObjects;
using SachNoiOnline.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace SachNoiOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IStoryRepository _storyRepository;

        public StoryController(IStoryRepository storyRepository)
        {
            _storyRepository = storyRepository;
        }

        // GET api/story/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var story = await _storyRepository.GetByIdAsync(id);

            if (story == null)
                return NotFound();

            return Ok(story);
        }

        // GET api/story
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<StoryResponse>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? authorId = null,         // Optional filter for AuthorId
            [FromQuery] int? categoryId = null,       // Optional filter for CategoryId
            [FromQuery] int? narratorId = null,       // Optional filter for NarratorId
            [FromQuery] string searchQuery = null     // Optional search query
        )
        {
            // Create pagination object
            var pagination = new Pagination(pageNumber, pageSize);

            // Create filter object
            var filter = new StoryFilter
            {
                AuthorId = authorId,
                CategoryId = categoryId,
                NarratorId = narratorId
            };

            // Fetch the paginated stories with filters and search query
            var paginatedStories = await _storyRepository.GetPaginatedAsync(pagination, filter, searchQuery);

            // Return the paginated response
            return Ok(paginatedStories);
        }


        // POST api/story
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] StoryRequest storyRequest)
        {
            if (storyRequest == null)
                return BadRequest("Invalid story data.");

            var createdStory = await _storyRepository.AddAsync(storyRequest);
            return Ok(createdStory);
        }

        // PUT api/story/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] StoryRequest storyRequest)
        {
            if (storyRequest == null)
                return BadRequest("Invalid story data.");

            if (storyRequest.Id != id)
                return BadRequest("ID in the request body does not match the route ID.");

            try
            {
                var updatedStory = await _storyRepository.UpdateAsync(storyRequest);

                if (updatedStory == null)
                    return NotFound($"Story with ID {id} not found.");

                return Ok(updatedStory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the story: {ex.Message}");
            }
        }


        // DELETE api/story/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _storyRepository.DeleteAsync(id);
            return NoContent();
        }

        // DELETE api/story/soft/{id} for soft delete
        [HttpDelete("soft/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _storyRepository.SoftDeleteAsync(id);
            return NoContent();
        }
    }
}
