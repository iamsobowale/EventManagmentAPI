using Domain.Entities;

namespace Domain.Repositories;

public interface IUserRepository
{
    public Task<bool> AddAsync(User user);
    public Task<bool> UpdateAsync(User user);
    public Task<bool> DeleteAsync(User user);
    public Task<User> GetByIdAsync(Guid id);
    public Task<IEnumerable<User>> GetAllAsync();
    public Task<bool> EmailExists(string email);
    public Task<User> GetByEmailAsync(string email);
    public Task<IEnumerable<User>> GetAllUsersByEventIdAsync(Guid eventId);
}