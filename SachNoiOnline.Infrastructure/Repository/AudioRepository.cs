using Microsoft.EntityFrameworkCore;
using SachNoiOnline.Domain.Entities;
using SachNoiOnline.Domain.Interfaces;
using SachNoiOnline.Infrastructure.EFCoreDbContext;
using SachNoiOnline.Domain.Responses;
using SachNoiOnline.Domain.Requests;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SachNoiOnline.Domain.ValueObjects;

namespace SachNoiOnline.Infrastructure.Repository
{
    public interface IAudioRepository
    {
        Task<AudioResponse> GetByIdAsync(int id);
        Task<PaginatedResponse<AudioResponse>> GetPaginatedAsync(Pagination pagination, int? storyId = null, string search = null);
        Task<AudioResponse> AddAsync(AudioRequest audioRequest);
        Task<AudioResponse> UpdateAsync(AudioRequest audioRequest);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
    }

    public class AudioRepository : IAudioRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Audio> _dbSet;
        private readonly IMapper _mapper;

        public AudioRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = context.Set<Audio>();
            _mapper = mapper;
        }

        // Get audio by ID
        public async Task<AudioResponse> GetByIdAsync(int id)
        {
            var audio = await _dbSet
                .Include(a => a.Story) // Include related story if necessary
                .FirstOrDefaultAsync(a => a.Id == id);

            if (audio == null)
                return null;

            return _mapper.Map<AudioResponse>(audio);
        }

        // Get audio with pagination
        public async Task<PaginatedResponse<AudioResponse>> GetPaginatedAsync(Pagination pagination, int? storyId = null, string search = null)
        {
            var query = _dbSet.AsQueryable();

            // Filter by storyId if provided
            if (storyId.HasValue)
            {
                query = query.Where(a => a.StoryId == storyId);
            }

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => EF.Functions.Like(a.Title, $"%{search}%"));
            }

            // Filter out soft-deleted audio
            query = query.Where(a => a.DeletedAt == null);

            var totalCount = await query.CountAsync();

            var audios = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize)
                                    .Take(pagination.PageSize)
                                    .Include(a => a.Story) // Include related story if necessary
                                    .ToListAsync();

            var audioResponses = _mapper.Map<List<AudioResponse>>(audios);

            return new PaginatedResponse<AudioResponse>(audioResponses, totalCount, pagination.PageNumber, pagination.PageSize);
        }


        // Add a new audio
        public async Task<AudioResponse> AddAsync(AudioRequest audioRequest)
        {
            var audio = _mapper.Map<Audio>(audioRequest);
            audio.CreatedAt = DateTime.UtcNow;
            audio.UpdatedAt = DateTime.UtcNow;

            // Handle audio file upload (for example, save to a folder and store URL)
            var audioFileUrl = await SaveAudioFileAsync(audioRequest.AudioFile);
            audio.AudioFileUrl = audioFileUrl;

            await _dbSet.AddAsync(audio);
            await SaveChangesAsync();

            return _mapper.Map<AudioResponse>(audio);
        }

        public async Task<AudioResponse> UpdateAsync(AudioRequest audioRequest)
        {
            var audio = await _dbSet.FirstOrDefaultAsync(a => a.Id == audioRequest.Id);
            if (audio == null)
                return null;

            try
            {
                // Map changes to the audio
                _mapper.Map(audioRequest, audio);
                audio.UpdatedAt = DateTime.UtcNow;

                // Handle audio file update if provided
                if (audioRequest.AudioFile != null)
                {
                    // Xóa file âm thanh cũ nếu tồn tại
                    if (!string.IsNullOrEmpty(audio.AudioFileUrl))
                    {
                        var oldFilePath = Path.Combine("wwwroot", audio.AudioFileUrl.TrimStart('/'));
                        if (File.Exists(oldFilePath))
                        {
                            File.Delete(oldFilePath);
                        }
                    }

                    // Lưu file âm thanh mới
                    var audioFileUrl = await SaveAudioFileAsync(audioRequest.AudioFile);
                    audio.AudioFileUrl = audioFileUrl;
                }

                _dbSet.Update(audio);
                await SaveChangesAsync();

                return _mapper.Map<AudioResponse>(audio);
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflict
                throw new Exception("The record was updated by another user. Please reload and try again.");
            }
        }


        public async Task DeleteAsync(int id)
        {
            var audio = await _dbSet.FirstOrDefaultAsync(a => a.Id == id);
            if (audio != null)
            {
                // Xóa file âm thanh nếu tồn tại
                if (!string.IsNullOrEmpty(audio.AudioFileUrl))
                {
                    var filePath = Path.Combine("wwwroot", audio.AudioFileUrl.TrimStart('/'));
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                // Xóa bản ghi khỏi cơ sở dữ liệu
                _dbSet.Remove(audio);
                await SaveChangesAsync();
            }
        }

        // Soft delete an audio (set DeletedAt to current time)
        public async Task SoftDeleteAsync(int id)
        {
            var audio = await GetByIdAsync(id);

            if (audio != null)
            {
                var audioEntity = _mapper.Map<Audio>(audio);
                audioEntity.DeletedAt = DateTime.UtcNow;
                _dbSet.Update(audioEntity);

                await SaveChangesAsync();
            }
        }

        // Helper method to save audio file and return its URL
        private async Task<string> SaveAudioFileAsync(IFormFile audioFile)
        {
            // Generate a unique filename for the audio file
            var fileName = $"{Guid.NewGuid()}_{audioFile.FileName}";

            // Define the relative file path in the /audio directory
            var filePath = Path.Combine("wwwroot", "audio", fileName);

            // Ensure the directory exists
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Save the audio file to the filesystem
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await audioFile.CopyToAsync(fileStream);
            }

            // Return the URL of the audio file, without the 'wwwroot' part
            return $"/audio/{fileName}";
        }


        // Helper method to save changes to the database
        private async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
