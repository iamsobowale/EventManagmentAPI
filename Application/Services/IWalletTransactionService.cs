using Application.DTOS;

namespace Application.Services;

public interface IWalletTransactionService
{
    public Task<BaseResponse<WalletTransactionDTO>> AddAsync(WalletTransactionDTO walletTransaction);
    public Task<BaseResponse<WalletTransactionDTO>> GetByIdAsync(Guid id);
    public Task<BaseResponse<IEnumerable<WalletTransactionDTO>>> GetAllByWalletIdAsync();
}