using Application.DTOS;
using Application.Identity;
using Application.Services;
using Domain.Repositories;

namespace Infrastructure.Persistence.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepostiory;
    private readonly IIdentityService _identityService;
    
    public AuthService(IUserRepository userRepostiory, IIdentityService identityService)
    {
        _userRepostiory = userRepostiory;
        _identityService = identityService;
    }
    public async Task<BaseResponse<LoginResponseDTO>> LoginAsync(LoginDTO login)
    {
        var user = await _userRepostiory.GetByEmailAsync(login.Email);
        if (user == null)
        {
            return new BaseResponse<LoginResponseDTO>
            {
                Message = "Invalid email or password",
                Data = null,
                Success = false
            };
        }
        if (user.Password != login.Password)
        {
            return new BaseResponse<LoginResponseDTO>
            {
                Message = "Invalid email or password",
                Data = null,
                Success = false
            };
        }
        return new BaseResponse<LoginResponseDTO>
        {
            Message = "Login successful",
            Data = new LoginResponseDTO
            {
                Email = user.Email,
                Role = user.Role,
                Username = user.Username,
                UserId = user.Id,
                Balance = user.Wallet.Balance,
                Token = _identityService.GenerateToken(user)
            },
            Success = true
        };
        
    }
}