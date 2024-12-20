using Microsoft.AspNetCore.Mvc;
using SachNoiOnline.Infrastructure.Repository;
using SachNoiOnline.Domain.Requests;
using SachNoiOnline.Domain.Responses;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SachNoiOnline.Domain.ValueObjects;

namespace SachNoiOnline.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudioController : ControllerBase
    {
        private readonly IAudioRepository _audioRepository;
        private readonly ILogger<AudioController> _logger;

        public AudioController(IAudioRepository audioRepository, ILogger<AudioController> logger)
        {
            _audioRepository = audioRepository;
            _logger = logger;
        }

        // GET: api/Audio/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var audio = await _audioRepository.GetByIdAsync(id);
            if (audio == null)
            {
                return NotFound(new { message = "Audio not found." });
            }

            return Ok(audio);
        }

        // GET: api/Audio
        // GET: api/Audio
        [HttpGet]
        public async Task<IActionResult> GetPaginated([FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10, [FromQuery] int? storyId = null, [FromQuery] string? search = null)
        {
            try
            {
                // Create pagination object
                var pagination = new Pagination(pageNumber, pageSize);

                var response = await _audioRepository.GetPaginatedAsync(pagination, storyId, search);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching paginated audio.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }


        // POST: api/Audio
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] AudioRequest audioRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var audio = await _audioRepository.AddAsync(audioRequest);
            return CreatedAtAction(nameof(GetById), new { id = audio.Id }, audio);
        }

        // PUT: api/Audio/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] AudioRequest audioRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != audioRequest.Id)
            {
                return BadRequest(new { message = "ID mismatch." });
            }

            var updatedAudio = await _audioRepository.UpdateAsync(audioRequest);
            if (updatedAudio == null)
            {
                return NotFound(new { message = "Audio not found." });
            }

            return Ok(updatedAudio);
        }

        // DELETE: api/Audio/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _audioRepository.DeleteAsync(id);
            return NoContent();
        }

        // PATCH: api/Audio/{id}/soft-delete
        [HttpPatch("{id}/soft-delete")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _audioRepository.SoftDeleteAsync(id);
            return NoContent();
        }
    }
}

