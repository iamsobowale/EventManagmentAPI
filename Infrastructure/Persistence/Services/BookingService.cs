using Application.DTOS;
using Application.Services;
using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Persistence.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletTransactionRepository _walletTransactionRepository;
    
    public BookingService(IBookingRepository bookingRepository, IUserRepository userRepository, IEventRepository eventRepository,IWalletRepository walletRepository, IWalletTransactionRepository walletTransactionRepository)
    {
        _bookingRepository = bookingRepository;
        _userRepository = userRepository;
        _eventRepository = eventRepository;
        _walletRepository = walletRepository;
        _walletTransactionRepository = walletTransactionRepository;
    }
    
    public async Task<BaseResponse<BookingDTO>> AddAsync(CreateBookingDTO booking)
    {
        var getEvent = await _eventRepository.GetByIdAsync(booking.EventId);
        var getuser = await _userRepository.GetByIdAsync(booking.UserId);
        if (getEvent == null)
        {
            return new BaseResponse<BookingDTO>
            {
                Data = null,
                Message = "Event not found",
                Success = false
            };
        }
        var userHasBooked = await CheckIfUserHasBookedEvent(booking.UserId, booking.EventId);
        if (userHasBooked)
        {
            return new BaseResponse<BookingDTO>
            {
                Data = null,
                Message = "User has already booked this event",
                Success = false
            };
        }
        if (!await BalanceIsEnough(getuser.Wallet.Balance, getEvent.Price))
        {
            return new BaseResponse<BookingDTO>
            {
                Data = null,
                Message = "Insufficient balance, Fund wallet to book event",
                Success = false
            };
        }

        var eventBookings = await GetAllBookingsByEventIdAsync(booking.EventId);
        if (getEvent.TotalTickets == eventBookings.Data?.Count())
        {
            return new BaseResponse<BookingDTO>
            {
                Data = null,
                Message = "Event is fully booked",
                Success = false
            };
        }

        var createBooking = new Booking
        {
            Id = Guid.NewGuid(),
            UserId = booking.UserId,
            EventId = booking.EventId,
            Event = getEvent,
            BookingDate = DateTime.Now
        };
        getuser.Wallet.Balance -= getEvent.Price;
        await _walletRepository.UpdateAsync(getuser.Wallet);
        var created = await _bookingRepository.AddAsync(createBooking);
        if (!created)
        {
            return new BaseResponse<BookingDTO>
            {
                Data = null,
                Message = "Failed to create booking",
                Success = false
            };
        }
        var createdWalletTransaction = new WalletTransaction
        {
            Id = Guid.NewGuid(),
            Amount = getEvent.Price,
            WalletId = getuser.Wallet.Id,
            TransactionType = "Debit",
            Description = $"{getEvent.Name} Event",
            TransactionDate = DateTime.Now
        };
        await _walletTransactionRepository.AddAsync(createdWalletTransaction);
        return new BaseResponse<BookingDTO>
        {
            Data = new BookingDTO
            {
                Id = createBooking.Id,
                UserId = createBooking.UserId,
                EventId = createBooking.EventId,
                BookingDate = createBooking.BookingDate,
                Price = getEvent.Price,
                EventName = getEvent.Name
            },
            Message = "Booking created successfully",
            Success = true
        };
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id)
    {
        var getBooking = await _bookingRepository.GetByIdAsync(id);
        if (getBooking == null)
        {
            return new BaseResponse<bool>
            {
                Data = false,
                Message = "Booking not found",
                Success = false
            };
        }
        var deleted = await _bookingRepository.DeleteAsync(getBooking);
        if (!deleted)
        {
            return new BaseResponse<bool>
            {
                Data = false,
                Message = "Failed to delete booking",
                Success = false
            };
        }
        getBooking.User.Wallet.Balance += getBooking.Event.Price;
        await _walletRepository.UpdateAsync(getBooking.User.Wallet);
        var createdWalletTransaction = new WalletTransaction
        {
            Id = Guid.NewGuid(),
            Amount = getBooking.Event.Price,
            WalletId = getBooking.User.Wallet.Id,
            TransactionType = "Credit",
            Description = $"Refund for {getBooking.Event.Name} Event",
            TransactionDate = DateTime.Now
        };
        await _walletTransactionRepository.AddAsync(createdWalletTransaction);
        return new BaseResponse<bool>
        {
            Data = true,
            Message = "Booking deleted successfully",
            Success = true
        };
    }

    public async Task<BaseResponse<BookingDTO>> GetByIdAsync(Guid id)
    {
        var getBooking = await _bookingRepository.GetByIdAsync(id);
        if (getBooking == null)
        {
            return new BaseResponse<BookingDTO>
            {
                Data = null,
                Message = "Booking not found",
                Success = false
            };
        }
        return new BaseResponse<BookingDTO>
        {
            Data = new BookingDTO
            {
                Id = getBooking.Id,
                UserId = getBooking.UserId,
                EventId = getBooking.EventId,
                BookingDate = getBooking.BookingDate,
                Price = getBooking.Event.Price,
                EventName = getBooking.Event.Name,
            },
            Message = "Booking found",
            Success = true
        };
    }

    public async Task<BaseResponse<IEnumerable<BookingDTO>>> GetAllAsync()
    {
        var bookings = await _bookingRepository.GetAllAsync();
        if (!bookings.Any())
        {
            return new BaseResponse<IEnumerable<BookingDTO>>
            {
                Data = null,
                Message = "No bookings found",
                Success = false
            };
        }
        return new BaseResponse<IEnumerable<BookingDTO>>
        {
            Data = bookings.Select(b => new BookingDTO
            {
                Id = b.Id,
                UserId = b.UserId,
                EventId = b.EventId,
            }).ToList(),
            Message = "Bookings found",
            Success = true
        };
    }

    public async Task<BaseResponse<IEnumerable<BookingDTO>>> GetAllBookingsByEventIdAsync(Guid eventId)
    {
        var bookings = await _bookingRepository.GetAllBookingsByEventIdAsync(eventId);
        if (!bookings.Any())
        {
            return new BaseResponse<IEnumerable<BookingDTO>>
            {
                Data = null,
                Message = "No bookings found",
                Success = false
            };
        }
        return new BaseResponse<IEnumerable<BookingDTO>>
        {
            Data = bookings.Select(b => new BookingDTO
            {
                Id = b.Id,
                UserId = b.UserId,
                EventId = b.EventId,
            }).ToList(),
            Message = "Bookings found",
            Success = true
        };
    }

    public async Task<bool> CheckIfUserHasBookedEvent(Guid userId, Guid eventId)
    {
        var bookings = await _bookingRepository.GetAllBookingsByUserIdAsync(userId);
        if (bookings == null)
        {
            return false;
        }
        var userHasBooked = bookings.Any(b => b.EventId == eventId);
        return userHasBooked;
    }

    public async Task<BaseResponse<IEnumerable<BookingDTO>>> GetAllBookingsByUserIdAsync(Guid userId)
    {
        var bookings = await _bookingRepository.GetAllBookingsByUserIdAsync(userId);
        if (!bookings.Any())
        {
            return new BaseResponse<IEnumerable<BookingDTO>>
            {
                Data = null,
                Message = "No bookings found",
                Success = false
            };
        }
        return new BaseResponse<IEnumerable<BookingDTO>>
        {
            Data = bookings.Select(b => new BookingDTO
            {
                Id = b.Id,
                UserId = b.UserId,
                EventId = b.EventId,
                EventName = b.Event.Name,
                Price = b.Event.Price,
                Event = new EventDTO
                {
                    Price = b.Event.Price,
                    Name = b.Event.Name,
                    Date = b.Event.Date,
                    Location = b.Event.Location,
                    Description = b.Event.Description,
                }
            }).ToList(),
            Message = "Bookings found",
            Success = true
        };
    }
    private async Task<bool> BalanceIsEnough(decimal userBalance, decimal eventPrice)
    {
        if (userBalance < eventPrice)
        {
            return false;
        }
        return true;
    }
}