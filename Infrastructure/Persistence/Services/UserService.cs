using Application.DTOS;
using Application.Services;
using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Persistence.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepostiory;
    private readonly IWalletService _walletService;
    
    public UserService(IUserRepository userRepostiory, IWalletService walletService)
    {
        _userRepostiory = userRepostiory;
        _walletService = walletService;
    }
    public async Task<BaseResponse<UserDTO>> AddAsync(CreateUserResponseDTO user)
    {
        var emailExist =await _userRepostiory.EmailExists(user.Email);
        if (emailExist)
        {
            return new BaseResponse<UserDTO>
            {
                Message = "Email already exists",
                Data = null,
                Success = false
            };
        }
        var createuser = new User
        {
            Id = Guid.NewGuid(),
            Email = user.Email,
            Role = "User",
            Gender = user.Gender,
            Password = user.Password,
            Username = user.Username
        };
        var created = await _userRepostiory.AddAsync(createuser);
        if (!created)
        {
            return new BaseResponse<UserDTO>
            {
                Message = "Failed to create user",
                Data = null,
                Success = false
            };
        }
        var createWallet = new WalletDTO()
        {
            Id = Guid.NewGuid(),
            UserId = createuser.Id,
            Balance = 0
        };
        var createdWallet = await _walletService.AddAsync(createWallet);
        return new BaseResponse<UserDTO>
        {
            Message = "User created successfully",
            Data = new UserDTO
            {
                Email = createuser.Email,
                Id = createuser.Id,
                Username = createuser.Username
            },
            Success = true
        };
    }

    public async Task<BaseResponse<UserDTO>> UpdateAsync(CreateUserResponseDTO user)
    {
        var getUser = await _userRepostiory.GetByIdAsync(user.Id);
        if (getUser == null)
        {
            return new BaseResponse<UserDTO>
            {
                Message = "User not found",
                Data = null,
                Success = false
            };
        }
        getUser.Email = user.Email;
        getUser.Username = user.Username;
        getUser.Password = user.Password;
        var updated = await _userRepostiory.UpdateAsync(getUser);
        if (!updated)
        {
            return new BaseResponse<UserDTO>
            {
                Message = "Failed to update user",
                Data = null,
                Success = false
            };
        }
        return new BaseResponse<UserDTO>
        {
            Message = "User updated successfully",
            Data = new UserDTO
            {
                Email = getUser.Email,
                Id = getUser.Id,
                Username = getUser.Username
            },
            Success = true
        };
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id)
    {
        var getUser = await _userRepostiory.GetByIdAsync(id);
        if (getUser == null)
        {
            return new BaseResponse<bool>
            {
                Message = "User not found",
                Data = false,
                Success = false
            };
        }
        var deleted = await _userRepostiory.DeleteAsync(getUser);
        if (!deleted)
        {
            return new BaseResponse<bool>
            {
                Message = "Failed to delete user",
                Data = false,
                Success = false
            };
        }
        return new BaseResponse<bool>
        {
            Message = "User deleted successfully",
            Data = false,
            Success = true
        };
    }

    public async Task<BaseResponse<UserDTO>> GetByIdAsync(Guid id)
    {
        var user = await _userRepostiory.GetByIdAsync(id);
        
        if (user == null)
        {
            return new BaseResponse<UserDTO>
            {
                Message = "User not found",
                Data = null,
                Success = false
            };
        }
        return new BaseResponse<UserDTO>
        {
            Message = "User found",
            Data = new UserDTO
            {
                Email = user.Email,
                Id = user.Id,
                Username = user.Username,
                Gender = user.Gender,
                Wallet = new WalletDTO
                {
                    Balance = user.Wallet.Balance,
                    Id = user.Wallet.Id
                }
            },
            Success = true
        };
    }

    public async Task<BaseResponse<IEnumerable<UserDTO>>> GetAllAsync()
    {
        var users = await _userRepostiory.GetAllAsync();
        if (!users.Any())
        {
            return new BaseResponse<IEnumerable<UserDTO>>
            {
                Message = "No user found",
                Data = null,
                Success = false
            };
        }
        return new BaseResponse<IEnumerable<UserDTO>>
        {
            Message = "Users found",
            Data = users.Select(user => new UserDTO
            {
                Email = user.Email,
                Id = user.Id,
                Username = user.Username,
                Gender = user.Gender
            }),
            Success = true
        };
    }

    public async Task<BaseResponse<IEnumerable<UserDTO>>> GetAllUsersByEventIdAsync(Guid eventId)
    {
        var users = await _userRepostiory.GetAllUsersByEventIdAsync(eventId);
        if (!users.Any())
        {
            return new BaseResponse<IEnumerable<UserDTO>>
            {
                Message = "No user found",
                Data = null,
                Success = false
            };
        }
        return new BaseResponse<IEnumerable<UserDTO>>
        {
            Message = "Users found",
            Data = users.Select(user => new UserDTO
            {
                Email = user.Email,
                Id = user.Id,
                Username = user.Username,
                Gender = user.Gender,
            }),
            Success = true
        };
    }
}