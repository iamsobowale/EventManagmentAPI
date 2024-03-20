using Application.DTOS;
using Domain.Entities;

namespace Application.Services;

public interface IUserService
{
    public Task<BaseResponse<UserDTO>> AddAsync(CreateUserResponseDTO user);
    public Task<BaseResponse<UserDTO>> UpdateAsync(CreateUserResponseDTO user);
    public Task<BaseResponse<bool>> DeleteAsync(Guid id);
    public Task<BaseResponse<UserDTO>> GetByIdAsync(Guid id);
    public Task<BaseResponse<IEnumerable<UserDTO>>> GetAllAsync();
    public Task<BaseResponse<IEnumerable<UserDTO>>> GetAllUsersByEventIdAsync(Guid eventId);
}