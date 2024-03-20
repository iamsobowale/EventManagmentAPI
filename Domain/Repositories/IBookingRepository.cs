using Domain.Entities;

namespace Domain.Repositories;

public interface IBookingRepository
{
    public Task<bool> AddAsync(Booking booking);
    public Task<bool> UpdateAsync(Booking booking);
    public Task<bool> DeleteAsync(Booking booking);
    public Task<Booking> GetByIdAsync(Guid id);
    public Task<IEnumerable<Booking>> GetAllAsync();
    public Task<IEnumerable<Booking>> GetAllBookingsByEventIdAsync(Guid eventId);
    public Task<IEnumerable<Booking>> GetAllBookingsByUserIdAsync(Guid userId);
}