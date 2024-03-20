using Application.DTOS;

namespace Application.Services;

public interface IEventService
{
    public Task<BaseResponse<EventDTO>> AddAsync(EventDTO @event);
    public Task<BaseResponse<EventDTO>> UpdateAsync(EventDTO @event);
    public Task<BaseResponse<EventDTO>> GetByIdAsync(Guid id);
    public Task<BaseResponse<IEnumerable<EventDTO>>> GetAllAsync();
    public Task<BaseResponse<bool>> DeleteAsync(Guid id);
    public Task<BaseResponse<bool>> ValidateEventTimeAsync(DateTime eventTime);
}