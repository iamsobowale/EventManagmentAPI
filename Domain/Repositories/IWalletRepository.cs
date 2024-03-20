using Domain.Entities;

namespace Domain.Repositories;

public interface IWalletRepository
{
    public Task<Wallet> GetByUserIdAsync(Guid userId);
    public Task<bool> AddAsync(Wallet wallet);
    public Task<bool> UpdateAsync(Wallet wallet);
    public Task<bool> DeleteAsync(Wallet wallet);
}