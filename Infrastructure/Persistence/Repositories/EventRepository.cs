using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class EventRepository : IEventRepository
{
    private readonly ApplicationContext _context;
    public EventRepository(ApplicationContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Event>> GetAllAsync()
    {
        return await _context.Events.Where(c => c.IsDeleted == false).ToListAsync();
    }

    public async Task<Event> GetByIdAsync(Guid id)
    {
        return await _context.Events.FindAsync(id);
    }

    public async Task<bool> AddAsync(Event @event)
    {
        await _context.Events.AddAsync(@event);
        var created = await _context.SaveChangesAsync();
        return created > 0;
    }

    public async Task<bool> UpdateAsync(Event @event)
    {
        _context.Events.Update(@event);
        var updated = await _context.SaveChangesAsync();
        return updated > 0;
    }

    public async Task<bool> DeleteAsync(Event @event)
    {
        @event.IsDeleted = true;
        var deleted = await _context.SaveChangesAsync();
        return deleted > 0;
    }
}