
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class WalletRepository : IWalletRepository
{
    private readonly ApplicationContext _context;

    public WalletRepository(ApplicationContext context)
    {
        _context = context;
    }
    public async Task<Wallet> GetByUserIdAsync(Guid userId)
    {
        return await _context.Wallets.FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<bool> AddAsync(Wallet wallet)
    {
        await _context.AddAsync(wallet);
        var created = await _context.SaveChangesAsync();
        return created > 0;
    }

    public async Task<bool> UpdateAsync(Wallet wallet)
    {
        _context.Wallets.Update(wallet);
        var updated = await _context.SaveChangesAsync();
        return updated > 0;
    }

    public async Task<bool> DeleteAsync(Wallet wallet)
    {
         wallet.IsDeleted = true;
        var deleted = await _context.SaveChangesAsync();
        return deleted > 0;
    }
}