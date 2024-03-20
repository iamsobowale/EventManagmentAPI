using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.DTOS;

public class BookingDTO
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "User ID is required")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Event ID is required")]
    public Guid EventId { get; set; }

    [Required(ErrorMessage = "Booking date is required")]
    [DataType(DataType.DateTime)]
    public DateTime BookingDate { get; set; }
    
    public UserDTO User { get; set; }
    
    public EventDTO Event { get; set; } 
    public decimal Price { get; set; }
    public string EventName { get; set; }
}

public class CreateBookingDTO
{
    [Required(ErrorMessage = "User ID is required")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Event ID is required")]
    public Guid EventId { get; set; }
}