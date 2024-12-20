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
    public interface IAuthorRepository
    {
        Task<AuthorResponse> GetByIdAsync(int id);
        Task<PaginatedResponse<AuthorResponse>> GetPaginatedAsync(Pagination pagination);
        Task<AuthorResponse> AddAsync(AuthorRequest authorRequest);
        Task<AuthorResponse> UpdateAsync(AuthorRequest authorRequest);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
    }

    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Author> _dbSet;
        private readonly IMapper _mapper;

        public AuthorRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = context.Set<Author>();
            _mapper = mapper;
        }

        // Get author by ID
        public async Task<AuthorResponse> GetByIdAsync(int id)
        {
            var author = await _dbSet
                .Include(a => a.Stories) // Include related stories if needed
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
                return null;

            return _mapper.Map<AuthorResponse>(author);
        }

        // Get authors with pagination
        public async Task<PaginatedResponse<AuthorResponse>> GetPaginatedAsync(Pagination pagination)
        {
            var query = _dbSet.AsQueryable();

            // Filter out soft-deleted authors
            query = query.Where(a => a.DeletedAt == null);

            var totalCount = await query.CountAsync();

            var authors = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize)
                                      .Take(pagination.PageSize)
                                      .Include(a => a.Stories) // Include related stories if needed
                                      .ToListAsync();

            var authorResponses = _mapper.Map<List<AuthorResponse>>(authors);

            return new PaginatedResponse<AuthorResponse>(authorResponses, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        // Add a new author
        public async Task<AuthorResponse> AddAsync(AuthorRequest authorRequest)
        {
            var author = _mapper.Map<Author>(authorRequest);
            author.CreatedAt = DateTime.UtcNow;
            author.UpdatedAt = DateTime.UtcNow;

            await _dbSet.AddAsync(author);
            await SaveChangesAsync();

            return _mapper.Map<AuthorResponse>(author);
        }

        // Update an existing author
        public async Task<AuthorResponse> UpdateAsync(AuthorRequest authorRequest)
        {
            var author = await _dbSet.FirstOrDefaultAsync(a => a.Id == authorRequest.Id);
            if (author == null)
                return null;

            try
            {
                // Map changes to the author
                _mapper.Map(authorRequest, author);
                author.UpdatedAt = DateTime.UtcNow;

                _dbSet.Update(author);
                await SaveChangesAsync();

                return _mapper.Map<AuthorResponse>(author);
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflict
                throw new Exception("The record was updated by another user. Please reload and try again.");
            }
        }

        // Delete an author
        public async Task DeleteAsync(int id)
        {
            var author = await GetByIdAsync(id);
            if (author != null)
            {
                _dbSet.Remove(await _dbSet.FindAsync(id));
                await SaveChangesAsync();
            }
        }

        // Soft delete an author (set DeletedAt to current time)
        public async Task SoftDeleteAsync(int id)
        {
            var author = await GetByIdAsync(id);

            if (author != null)
            {
                var authorEntity = _mapper.Map<Author>(author);
                authorEntity.DeletedAt = DateTime.UtcNow;
                _dbSet.Update(authorEntity);

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
