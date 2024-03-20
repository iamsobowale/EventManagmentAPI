using Domain.Entities;

namespace Domain.Repositories;

public interface IWalletTransactionRepository
{
    public Task<bool> AddAsync(WalletTransaction walletTransaction);
    public Task<WalletTransaction> GetByIdAsync(Guid id);
    public Task<IEnumerable<WalletTransaction>> GetAllByWalletIdAsync();
}