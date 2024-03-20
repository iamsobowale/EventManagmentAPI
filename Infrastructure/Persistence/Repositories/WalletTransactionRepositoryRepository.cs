using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class WalletTransactionRepositoryRepository : IWalletTransactionRepository
{
    private readonly ApplicationContext _context;
    
    public WalletTransactionRepositoryRepository(ApplicationContext context)
    {
        _context = context;
    }
    public async Task<bool> AddAsync(WalletTransaction walletTransaction)
    {
        await _context.AddAsync(walletTransaction);
        var created = await _context.SaveChangesAsync();
        return created > 0;
    }

    public async Task<WalletTransaction> GetByIdAsync(Guid id)
    {
        return await _context.WalletTransactions.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<WalletTransaction>> GetAllByWalletIdAsync()
    {
        return await _context.WalletTransactions.ToListAsync();
    }
}