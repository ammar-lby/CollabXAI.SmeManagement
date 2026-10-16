using CollabXAI.Application.Interfaces;
using CollabXAI.Domain.Entities;
using CollabXAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CollabXAI.Infrastructure.Repositories;

// the only class that talks to the database. it implements the interface from the
// application layer, so the service never knows we are using ef core or sqlite
public class SmeRepository : ISmeRepository
{
    private readonly AppDbContext _context;

    // the dbcontext is injected by the framework, one per web request
    public SmeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Sme>> GetAllAsync()
    {
        // AsNoTracking: we only read these, so entity framework doesnt need to watch them for changes.
        // that makes the query a bit faster and use less memory
        return await _context.Smes
            .AsNoTracking()
            .OrderBy(s => s.OrganisationName)
            .ToListAsync();
    }

    public async Task<Sme?> GetByIdAsync(Guid id)
    {
        // returns null if there is no match, the service decides what to do about that
        return await _context.Smes.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null)
    {
        // true if a different sme already uses this email. excludeId is the sme being
        // updated, so it doesnt clash with itself. (null excludeId matches every row)
        return await _context.Smes.AnyAsync(s => s.ContactEmail == email && s.Id != excludeId);
    }

    public async Task AddAsync(Sme sme)
    {
        _context.Smes.Add(sme);

        // nothing reaches the database until SaveChangesAsync, that's when the INSERT runs
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
