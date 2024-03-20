using Application.DTOS;

namespace Application.Services;

public interface IAuthService
{
    public Task<BaseResponse<LoginResponseDTO>> LoginAsync(LoginDTO login);
}