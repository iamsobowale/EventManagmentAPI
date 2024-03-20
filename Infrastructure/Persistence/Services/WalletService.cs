using Application.DTOS;
using Application.Services;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Services;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;
    private readonly IPaystackRepository _paystackRepository;
    private readonly IWalletTransactionRepository _walletTransactionRepository;
    
    public WalletService(IWalletRepository walletRepository, IPaystackRepository paystackRepository, IWalletTransactionRepository walletTransactionRepository)
    {
        _walletRepository = walletRepository;
        _paystackRepository = paystackRepository;
        _walletTransactionRepository = walletTransactionRepository;
    }
    public async Task<BaseResponse<WalletDTO>> AddAsync(WalletDTO wallet)
    {
        var createWallet = new Wallet
        {
            Id = Guid.NewGuid(),
            UserId = wallet.UserId,
            Balance = 0
        };
        var created = await _walletRepository.AddAsync(createWallet);
        if (!created)
        {
            return new BaseResponse<WalletDTO>
            {
                Data = null,
                Message = "Failed to create wallet",
                Success = false
            };
        }
        var createWalletTransaction = new WalletTransaction
        {
            Id = Guid.NewGuid(),
            Amount = 0,
            WalletId = createWallet.Id,
            TransactionType = "Credit",
            Description = "Wallet created",
            TransactionDate = DateTime.Now
        };
        await _walletTransactionRepository.AddAsync(createWalletTransaction);
        return new BaseResponse<WalletDTO>
        {
            Data = new WalletDTO
            {
                Id = createWallet.Id,
                UserId = createWallet.UserId,
                Balance = createWallet.Balance
            },
            Message = "Wallet created successfully",
            Success = true
        };
    }

    public Task<BaseResponse<WalletDTO>> UpdateAsync(WalletDTO wallet)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<WalletDTO>> GetByUserIdAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<BaseResponse<FundWalletDTO>> FundWalletAsync(WalletDTO fundWallet)
    {
        var getWallet = await _walletRepository.GetByUserIdAsync(fundWallet.UserId);
        if (getWallet == null)
        {
            return new BaseResponse<FundWalletDTO>
            {
                Data = null,
                Message = "Wallet not found",
                Success = false
            };
        }
        var paystackResponse = await _paystackRepository.InitializeTransactionAsync(fundWallet.UserEmail, fundWallet.Balance);
        if (!paystackResponse.status)
        {
            return new BaseResponse<FundWalletDTO>
            {
                Data = null,
                Message = paystackResponse.message,
                Success = false
            };
        }
        getWallet.Balance += fundWallet.Balance;
        await _walletRepository.UpdateAsync(getWallet);
        var createWalletTransaction = new WalletTransaction
        {
            Id = Guid.NewGuid(),
            Amount = fundWallet.Balance,
            WalletId = getWallet.Id,
            TransactionType = "Credit",
            Description = "Wallet funded",
            TransactionDate = DateTime.Now
        };
        await _walletTransactionRepository.AddAsync(createWalletTransaction);
        
        return new BaseResponse<FundWalletDTO>
        {
            Data = new FundWalletDTO
            {
                UserId = getWallet.UserId,
                UserEmail = fundWallet.UserEmail,
                Amount = fundWallet.Balance,
                url = paystackResponse.data.authorization_url,
                reference = paystackResponse.data.reference
            },
            Success = true,
            Message = "Wallet Funded successfully"
        };
    }
}