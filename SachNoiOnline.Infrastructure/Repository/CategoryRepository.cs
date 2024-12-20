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
using SachNoiOnline.Domain.ValueObjects;

namespace SachNoiOnline.Infrastructure.Repository
{
    public interface ICategoryRepository
    {
        Task<CategoryResponse> GetByIdAsync(int id);
        Task<PaginatedResponse<CategoryResponse>> GetPaginatedAsync(Pagination pagination);
        Task<CategoryResponse> AddAsync(CategoryRequest categoryRequest);
        Task<CategoryResponse> UpdateAsync(CategoryRequest categoryRequest);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
    }

    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Category> _dbSet;
        private readonly IMapper _mapper;

        public CategoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = context.Set<Category>();
            _mapper = mapper;
        }

        // Get category by ID
        public async Task<CategoryResponse> GetByIdAsync(int id)
        {
            var category = await _dbSet
                .Include(c => c.Stories) // Include related stories if needed
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return null;

            return _mapper.Map<CategoryResponse>(category);
        }

        // Get categories with pagination
        public async Task<PaginatedResponse<CategoryResponse>> GetPaginatedAsync(Pagination pagination)
        {
            var query = _dbSet.AsQueryable();

            // Filter out soft-deleted categories
            query = query.Where(c => c.DeletedAt == null);

            var totalCount = await query.CountAsync();

            var categories = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize)
                                         .Take(pagination.PageSize)
                                         .Include(c => c.Stories) // Include related stories if needed
                                         .ToListAsync();

            var categoryResponses = _mapper.Map<List<CategoryResponse>>(categories);

            return new PaginatedResponse<CategoryResponse>(categoryResponses, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        // Add a new category
        public async Task<CategoryResponse> AddAsync(CategoryRequest categoryRequest)
        {
            var category = _mapper.Map<Category>(categoryRequest);
            category.CreatedAt = DateTime.UtcNow;
            category.UpdatedAt = DateTime.UtcNow;

            await _dbSet.AddAsync(category);
            await SaveChangesAsync();

            return _mapper.Map<CategoryResponse>(category);
        }

        // Update an existing category
        public async Task<CategoryResponse> UpdateAsync(CategoryRequest categoryRequest)
        {
            var category = await _dbSet.FirstOrDefaultAsync(c => c.Id == categoryRequest.Id);
            if (category == null)
                return null;

            try
            {
                // Map changes to the category
                _mapper.Map(categoryRequest, category);
                category.UpdatedAt = DateTime.UtcNow;

                _dbSet.Update(category);
                await SaveChangesAsync();

                return _mapper.Map<CategoryResponse>(category);
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflict
                throw new Exception("The record was updated by another user. Please reload and try again.");
            }
        }

        // Delete a category
        public async Task DeleteAsync(int id)
        {
            var category = await GetByIdAsync(id);
            if (category != null)
            {
                _dbSet.Remove(await _dbSet.FindAsync(id));
                await SaveChangesAsync();
            }
        }

        // Soft delete a category (set DeletedAt to current time)
        public async Task SoftDeleteAsync(int id)
        {
            var category = await GetByIdAsync(id);

            if (category != null)
            {
                var categoryEntity = _mapper.Map<Category>(category);
                categoryEntity.DeletedAt = DateTime.UtcNow;
                _dbSet.Update(categoryEntity);

                await SaveChangesAsync();
            }
        }

        // Helper method to save changes to the database
        private async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
