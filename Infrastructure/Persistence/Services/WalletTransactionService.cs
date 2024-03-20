using Application.DTOS;
using Application.Services;
using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Persistence.Services;

public class WalletTransactionService : IWalletTransactionService
{
    private readonly IWalletTransactionRepository _walletTransactionRepository;

    public WalletTransactionService(IWalletTransactionRepository walletTransactionRepository)
    {
        _walletTransactionRepository = walletTransactionRepository;
    }

    public async Task<BaseResponse<WalletTransactionDTO>> AddAsync(WalletTransactionDTO walletTransaction)
    {
        var newWalletTransaction = new WalletTransaction
        {
            Id = Guid.NewGuid(),
            Amount = walletTransaction.Amount,
            WalletId = walletTransaction.WalletId,
            TransactionType = walletTransaction.TransactionType,
            Description = walletTransaction.Description,
            TransactionDate = walletTransaction.TransactionDate,
        };
        var created = await _walletTransactionRepository.AddAsync(newWalletTransaction);
        if (!created)
        {
            return new BaseResponse<WalletTransactionDTO>()
            {
                Data = null,
                Message = "Failed to create wallet transaction",
                Success = false
            };
        }

        return new BaseResponse<WalletTransactionDTO>()
        {
            Data = walletTransaction,
            Message = "Wallet transaction created successfully",
            Success = true
        };
    }

    public async Task<BaseResponse<WalletTransactionDTO>> GetByIdAsync(Guid id)
    {
        var walletTransaction = await _walletTransactionRepository.GetByIdAsync(id);
        if (walletTransaction == null)
        {
            return new BaseResponse<WalletTransactionDTO>()
            {
                Data = null,
                Message = "Wallet transaction not found",
                Success = false
            };
        }

        return new BaseResponse<WalletTransactionDTO>()
        {
            Data = new WalletTransactionDTO
            {
                Id = walletTransaction.Id,
                Amount = walletTransaction.Amount,
                WalletId = walletTransaction.WalletId,
                TransactionType = walletTransaction.TransactionType,
                Description = walletTransaction.Description,
                TransactionDate = walletTransaction.TransactionDate,
            },
            Message = "Wallet transaction found",
            Success = true
        };
    }

    public async Task<BaseResponse<IEnumerable<WalletTransactionDTO>>> GetAllByWalletIdAsync()
    {
        var walletTransactions = await _walletTransactionRepository.GetAllByWalletIdAsync();
        if (!walletTransactions.Any())
        {
            return new BaseResponse<IEnumerable<WalletTransactionDTO>>()
            {
                Data = null,
                Message = "No wallet transactions found",
                Success = false
            };
        }

        var walletTransactionDTOs = walletTransactions.Select(c => new WalletTransactionDTO
        {
            Id = c.Id,
            Amount = c.Amount,
            WalletId = c.WalletId,
            TransactionType = c.TransactionType,
            Description = c.Description,
            TransactionDate = c.TransactionDate,
        });

        return new BaseResponse<IEnumerable<WalletTransactionDTO>>()
        {
            Data = walletTransactionDTOs,
            Message = "Wallet transactions found",
            Success = true
        };
    }
}