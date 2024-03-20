using Application.DTOS;
using Application.Services;
using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Persistence.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    public EventService(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }
    public async Task<BaseResponse<EventDTO>> AddAsync(EventDTO @event)
    {
        var validateEventTime = await ValidateEventTimeAsync(@event.Date);
        if (!validateEventTime.Success)
        {
            return new BaseResponse<EventDTO>
            {
                Message = validateEventTime.Message,
                Data = null,
                Success = false
            };
        }
        var createEvent = new Event
        {
            Id = Guid.NewGuid(),
            Name = @event.Name,
            Description = @event.Description,
            Date = @event.Date,
            Location = @event.Location,
            Price = @event.Price,
            TotalTickets = @event.TotalTickets
        };
        var created = await _eventRepository.AddAsync(createEvent);
        if (!created)
        {
            return new BaseResponse<EventDTO>
            {
                Message = "Failed to create event",
                Data = null,
                Success = false
            };
        }
        return new BaseResponse<EventDTO>
        {
            Message = "Event created successfully",
            Data = @event,
            Success = true
        };
    }

    public async Task<BaseResponse<EventDTO>> UpdateAsync(EventDTO @event)
    {
        var validateEventTime = await ValidateEventTimeAsync(@event.Date);
        if (!validateEventTime.Success)
        {
            return new BaseResponse<EventDTO>
            {
                Message = validateEventTime.Message,
                Data = null,
                Success = false
            };
        }
        var getEvent = await _eventRepository.GetByIdAsync(@event.Id);
        if (getEvent == null)
        {
            return new BaseResponse<EventDTO>
            {
                Message = "Event not found",
                Data = null,
                Success = false
            };
        }
        
        getEvent.Name = @event.Name;
        getEvent.Description = @event.Description;
        getEvent.Date = @event.Date;
        getEvent.Location = @event.Location;
        getEvent.Price = @event.Price;
        getEvent.TotalTickets = @event.TotalTickets;
        
        var updated = await _eventRepository.UpdateAsync(getEvent);
        if (!updated)
        {
            return new BaseResponse<EventDTO>
            {
                Message = "Failed to update event",
                Data = null,
                Success = false
            };
        }
        return new BaseResponse<EventDTO>
        {
            Message = "Event updated successfully",
            Data = @event,
            Success = true
        };
    }

    public async Task<BaseResponse<EventDTO>> GetByIdAsync(Guid id)
    {
        var getEvent = await _eventRepository.GetByIdAsync(id);
        if (getEvent == null)
        {
            return new BaseResponse<EventDTO>
            {
                Message = "Event not found",
                Data = null,
                Success = false
            };
        }
        var eventDTO = new EventDTO
        {
            Id = getEvent.Id,
            Name = getEvent.Name,
            Description = getEvent.Description,
            Date = getEvent.Date,
            Location = getEvent.Location,
            Price = getEvent.Price,
            TotalTickets = getEvent.TotalTickets
        };
        return new BaseResponse<EventDTO>
        {
            Message = "Event found",
            Data = eventDTO,
            Success = true
        };
    }

    public async Task<BaseResponse<IEnumerable<EventDTO>>> GetAllAsync()
    {
        var getEvents = await _eventRepository.GetAllAsync();
        if (!getEvents.Any())
        {
            return new BaseResponse<IEnumerable<EventDTO>>
            {
                Message = "No events found",
                Data = null,
                Success = false
            };
        }
        return new BaseResponse<IEnumerable<EventDTO>>()
        {
            Message = "Events found",
            Data = getEvents.Select(e => new EventDTO
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Date = e.Date,
                Location = e.Location,
                Price = e.Price,
                TotalTickets = e.TotalTickets
            }),
            Success = true
        };
    }
    
    public async Task<BaseResponse<bool>> DeleteAsync(Guid id)
    {
        var getEvent = await _eventRepository.GetByIdAsync(id);
        if (getEvent == null)
        {
            return new BaseResponse<bool>
            {
                Message = "Event not found",
                Data = false,
                Success = false
            };
        }
        var deleted = await _eventRepository.DeleteAsync(getEvent);
        if (!deleted)
        {
            return new BaseResponse<bool>
            {
                Message = "Failed to delete event",
                Data = false,
                Success = false
            };
        }
        return new BaseResponse<bool>
        {
            Message = "Event deleted successfully",
            Data = true,
            Success = true
        };
    }

    public async Task<BaseResponse<bool>> ValidateEventTimeAsync(DateTime eventTime)
    {
        if (eventTime < DateTime.Now)
        {
            return new BaseResponse<bool>
            {
                Message = "Event date cannot be in the past or today",
                Data = false,
                Success = false
            };
        }
        return new BaseResponse<bool>
        {
            Message = "Event date is valid",
            Data = true,
            Success = true
        };
    }
}