using Microsoft.EntityFrameworkCore;
using SachNoiOnline.Domain.Entities;
using SachNoiOnline.Domain.Interfaces;
using SachNoiOnline.Infrastructure.EFCoreDbContext;
using SachNoiOnline.Domain.Responses;
using SachNoiOnline.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SachNoiOnline.Domain.Requests;
using AutoMapper;

namespace SachNoiOnline.Infrastructure.Repository
{
    public interface INarratorRepository
    {
        Task<NarratorResponse> GetByIdAsync(int id);
        Task<PaginatedResponse<NarratorResponse>> GetPaginatedAsync(Pagination pagination);
        Task<NarratorResponse> AddAsync(NarratorRequest narratorRequest);
        Task<NarratorResponse> UpdateAsync(NarratorRequest narratorRequest);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
    }

    public class NarratorRepository : INarratorRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Narrator> _dbSet;
        private readonly IMapper _mapper;

        public NarratorRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = context.Set<Narrator>();
            _mapper = mapper;
        }

        public async Task<NarratorResponse> GetByIdAsync(int id)
        {
            var narrator = await _dbSet
                .Include(n => n.Stories) 
                .FirstOrDefaultAsync(n => n.Id == id);

            if (narrator == null)
                return null;

            return _mapper.Map<NarratorResponse>(narrator);
        }

        public async Task<PaginatedResponse<NarratorResponse>> GetPaginatedAsync(Pagination pagination)
        {
            var query = _dbSet.AsQueryable();

            var totalCount = await query.CountAsync();

            var narrators = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize)
                                        .Take(pagination.PageSize)
                                        .Include(n => n.Stories)  // Include related stories
                                        .ToListAsync();

            var narratorResponses = _mapper.Map<List<NarratorResponse>>(narrators);

            return new PaginatedResponse<NarratorResponse>(narratorResponses, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        // Add a new narrator
        public async Task<NarratorResponse> AddAsync(NarratorRequest narratorRequest)
        {
            var narrator = _mapper.Map<Narrator>(narratorRequest);
            narrator.CreatedAt = DateTime.UtcNow;
            narrator.UpdatedAt = DateTime.UtcNow;

            await _dbSet.AddAsync(narrator);
            await SaveChangesAsync();

            return _mapper.Map<NarratorResponse>(narrator);
        }

        public async Task<NarratorResponse> UpdateAsync(NarratorRequest narratorRequest)
        {
            var narrator = await _dbSet.FirstOrDefaultAsync(n => n.Id == narratorRequest.Id);
            if (narrator == null)
                return null;

            try
            {
                // Map changes
                _mapper.Map(narratorRequest, narrator);
                narrator.UpdatedAt = DateTime.UtcNow;

                _dbSet.Update(narrator);
                await SaveChangesAsync();

                return _mapper.Map<NarratorResponse>(narrator);
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflict here
                throw new Exception("The record was updated by another user. Please reload and try again.");
            }
        }


        // Delete a narrator
        public async Task DeleteAsync(int id)
        {
            var narrator = await GetByIdAsync(id);
            if (narrator != null)
            {
                _dbSet.Remove(await _dbSet.FindAsync(id));
                await SaveChangesAsync();
            }
        }

        // Soft delete a narrator (set DeletedAt to current time)
        public async Task SoftDeleteAsync(int id)
        {
            var narratorResponse = await GetByIdAsync(id);

            if (narratorResponse != null)
            {
                var narrator = _mapper.Map<Narrator>(narratorResponse);

                narrator.DeletedAt = DateTime.UtcNow;
                _dbSet.Update(narrator);

                await SaveChangesAsync();
            }
        }

        private async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
