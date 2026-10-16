using CollabXAI.Application.Interfaces;
using CollabXAI.Domain.Entities;
using CollabXAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CollabXAI.Infrastructure.Repositories;

public class SmeRepository : ISmeRepository
{
    private readonly AppDbContext _context;

    public SmeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Sme>> GetAllAsync()
    {
        // read only, so no need for ef to track changes
        return await _context.Smes
            .AsNoTracking()
            .OrderBy(s => s.OrganisationName)
            .ToListAsync();
    }

    public async Task<Sme?> GetByIdAsync(Guid id)
    {
        return await _context.Smes.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null)
    {
        return await _context.Smes.AnyAsync(s => s.ContactEmail == email && s.Id != excludeId);
    }

    public async Task AddAsync(Sme sme)
    {
        _context.Smes.Add(sme);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Sme sme)
    {
        _context.Smes.Update(sme);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Sme sme)
    {
        _context.Smes.Remove(sme);
        await _context.SaveChangesAsync();
    }
}
