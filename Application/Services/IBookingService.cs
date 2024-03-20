using Application.DTOS;

namespace Application.Services;

public interface IBookingService
{
    public Task<BaseResponse<BookingDTO>> AddAsync(CreateBookingDTO booking);
    public Task<BaseResponse<bool>> DeleteAsync(Guid id);
    public Task<BaseResponse<BookingDTO>> GetByIdAsync(Guid id);
    public Task<BaseResponse<IEnumerable<BookingDTO>>> GetAllAsync();
    public Task<BaseResponse<IEnumerable<BookingDTO>>> GetAllBookingsByEventIdAsync(Guid eventId);
    public Task<bool> CheckIfUserHasBookedEvent(Guid userId, Guid eventId);
    public Task<BaseResponse<IEnumerable<BookingDTO>>> GetAllBookingsByUserIdAsync(Guid userId);
}