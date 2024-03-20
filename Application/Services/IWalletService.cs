using Application.DTOS;

namespace Application.Services;

public interface IWalletService
{
    public Task<BaseResponse<WalletDTO>> AddAsync(WalletDTO wallet);
    public Task<BaseResponse<WalletDTO>> UpdateAsync(WalletDTO wallet);
    public Task<BaseResponse<WalletDTO>> GetByUserIdAsync(string userId);
    public Task<BaseResponse<FundWalletDTO>> FundWalletAsync(WalletDTO fundWallet);
}