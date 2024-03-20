using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationContext _context;

    public UserRepository(ApplicationContext context)
    {
        _context = context;
    } 
    public async Task<bool> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        var created = await _context.SaveChangesAsync();
        return created > 0;
    }

    public async Task<bool> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        var updated = await _context.SaveChangesAsync();
        return updated > 0;
    }

    public async Task<bool> DeleteAsync(User user)
    {
        user.IsDeleted = true;
        var deleted = await _context.SaveChangesAsync();
        return deleted > 0;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        return await _context.Users.Include(c => c.Wallet).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.Where(c => c.IsDeleted == false).ToListAsync();
    }

    public async Task<bool> EmailExists(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _context.Users.Include(c => c.Wallet).FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetAllUsersByEventIdAsync(Guid eventId)
    {
        return await _context.Users
            .Include(c => c.Wallet)
            .Include(c => c.Bookings)
            .Where(c => c.Bookings.Any(t => t.EventId == eventId))
            .ToListAsync();
    }
}