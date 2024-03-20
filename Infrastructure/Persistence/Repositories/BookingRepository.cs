using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationContext _context;
    public BookingRepository(ApplicationContext context)
    {
        _context = context;
    }
    public async Task<bool> AddAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
        var created = await _context.SaveChangesAsync();
        return created > 0;
    }

    public async Task<bool> UpdateAsync(Booking booking)
    {
        _context.Bookings.Update(booking);
        var updated = await _context.SaveChangesAsync();
        return updated > 0;
    }

    public async Task<bool> DeleteAsync(Booking booking)
    {
        booking.IsDeleted = true;
        var deleted = await _context.SaveChangesAsync();
        return deleted > 0;
    }

    public async Task<Booking> GetByIdAsync(Guid id)
    {
        return await _context.Bookings.Include(c =>c.Event).Include(c => c.User).ThenInclude(c => c.Wallet).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await _context.Bookings.Include(c =>c.Event).Where(c => c.IsDeleted == false).ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetAllBookingsByEventIdAsync(Guid eventId)
    {
        return await _context.Bookings.Include(c => c.Event).Where(c => c.EventId == eventId && c.IsDeleted == false).ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetAllBookingsByUserIdAsync(Guid userId)
    {
        return await _context.Bookings.Include(c => c.Event).Where(c => c.UserId == userId && c.IsDeleted == false).ToListAsync();
    }
}