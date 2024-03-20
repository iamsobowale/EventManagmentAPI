using Domain.Entities;

namespace Domain.Repositories;

public interface IEventRepository
{
    public Task<IEnumerable<Event>> GetAllAsync();
    public Task<Event> GetByIdAsync(Guid id);
    public Task<bool> AddAsync(Event @event);
    public Task<bool> UpdateAsync(Event @event);
    public Task<bool> DeleteAsync(Event @event);
}