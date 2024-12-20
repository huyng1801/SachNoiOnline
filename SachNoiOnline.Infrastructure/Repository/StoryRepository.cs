using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using SachNoiOnline.Domain.Entities;
using SachNoiOnline.Infrastructure.EFCoreDbContext;
using SachNoiOnline.Domain.Requests;
using SachNoiOnline.Domain.Responses;
using SachNoiOnline.Domain.ValueObjects;
using SachNoiOnline.Domain.Filters;

namespace SachNoiOnline.Infrastructure.Repositories
{
    public interface IStoryRepository
    {
        Task<StoryResponse> GetByIdAsync(int id);
        Task<PaginatedResponse<StoryResponse>> GetPaginatedAsync(Pagination pagination, StoryFilter filter, string searchQuery);
        Task<StoryResponse> AddAsync(StoryRequest storyRequest);
        Task<StoryResponse> UpdateAsync(StoryRequest storyRequest);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
    }
    public class StoryRepository : IStoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _coverImagePath;
        private readonly IMapper _mapper;

        public StoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _coverImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "coverImages");
        }

        // Upload cover image and return the file path
        public async Task<string> UploadCoverImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_coverImagePath, fileName);

            if (!Directory.Exists(_coverImagePath))
                Directory.CreateDirectory(_coverImagePath);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/coverImages/{fileName}";
        }

        // Get story by ID with all related data
        public async Task<StoryResponse> GetByIdAsync(int id)
        {
            var story = await _context.Stories
                .Include(s => s.Author)
                .Include(s => s.Category)
                .Include(s => s.Narrator)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (story == null)
                return null;

            // Use AutoMapper to map Story to StoryResponse
            var storyResponse = _mapper.Map<StoryResponse>(story);
            return storyResponse;
        }

        public async Task<PaginatedResponse<StoryResponse>> GetPaginatedAsync(Pagination pagination, StoryFilter filter, string searchQuery)
        {
            // Start with the base query and include related entities
            var query = _context.Stories
                .Include(s => s.Author)
                .Include(s => s.Category)
                .Include(s => s.Narrator)
                .AsQueryable();  // Ensure consistent IQueryable for filtering and paging

            // Apply filtering
            if (filter.AuthorId.HasValue)
            {
                query = query.Where(s => s.AuthorId == filter.AuthorId.Value);
            }
            if (filter.CategoryId.HasValue)
            {
                query = query.Where(s => s.CategoryId == filter.CategoryId.Value);
            }
            if (filter.NarratorId.HasValue)
            {
                query = query.Where(s => s.NarratorId == filter.NarratorId.Value);
            }

            // Apply search query
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(s => s.Title.Contains(searchQuery) || s.Description.Contains(searchQuery));
            }

            // Get total count after applying all filters and search query
            var totalCount = await query.CountAsync();

            // Apply pagination
            var stories = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            // Map stories to the response DTO
            var storyResponses = _mapper.Map<List<StoryResponse>>(stories);

            // Return paginated response with data, total count, and pagination details
            return new PaginatedResponse<StoryResponse>(storyResponses, totalCount, pagination.PageSize, pagination.PageNumber);
        }



        // Add a new story or update an existing story
        public async Task<StoryResponse> AddAsync(StoryRequest storyRequest)
        {
            var story = new Story
            {
                Title = storyRequest.Title,
                Description = storyRequest.Description,
                AuthorId = storyRequest.AuthorId,
                CategoryId = storyRequest.CategoryId,
                NarratorId = storyRequest.NarratorId,
                ListenersCount = 0, // Set default value
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            if (storyRequest.CoverImageFile != null)
            {
                story.CoverImageUrl = await UploadCoverImageAsync(storyRequest.CoverImageFile);
            }

            _context.Stories.Add(story);
            await _context.SaveChangesAsync();

            // Use AutoMapper to map Story to StoryResponse
            var storyResponse = _mapper.Map<StoryResponse>(story);
            return storyResponse;
        }

        // Update an existing story
        public async Task<StoryResponse> UpdateAsync(StoryRequest storyRequest)
        {
            var story = await _context.Stories.FindAsync(storyRequest.Id);
            if (story == null)
                return null;

            story.Title = storyRequest.Title;
            story.Description = storyRequest.Description;
            story.AuthorId = storyRequest.AuthorId;
            story.CategoryId = storyRequest.CategoryId;
            story.NarratorId = storyRequest.NarratorId;
            story.UpdatedAt = DateTime.UtcNow;

            if (storyRequest.CoverImageFile != null)
            {
                // Remove old image if it exists
                if (!string.IsNullOrEmpty(story.CoverImageUrl))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", story.CoverImageUrl.TrimStart('/'));
                    if (File.Exists(oldImagePath))
                        File.Delete(oldImagePath);
                }

                // Upload new image
                story.CoverImageUrl = await UploadCoverImageAsync(storyRequest.CoverImageFile);
            }

            _context.Stories.Update(story);
            await _context.SaveChangesAsync();

            // Use AutoMapper to map Story to StoryResponse
            var storyResponse = _mapper.Map<StoryResponse>(story);
            return storyResponse;
        }


        // Soft delete a story
        public async Task SoftDeleteAsync(int id)
        {
            var story = await _context.Stories.FindAsync(id);
            if (story != null)
            {
                story.DeletedAt = DateTime.UtcNow;
                _context.Stories.Update(story);
                await _context.SaveChangesAsync();
            }
        }

        // Hard delete a story
        public async Task DeleteAsync(int id)
        {
            var story = await _context.Stories.FindAsync(id);
            if (story != null)
            {
                // Remove image from disk
                if (!string.IsNullOrEmpty(story.CoverImageUrl))
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", story.CoverImageUrl.TrimStart('/'));
                    if (File.Exists(imagePath))
                        File.Delete(imagePath);
                }

                _context.Stories.Remove(story);
                await _context.SaveChangesAsync();
            }
        }

    }
}
