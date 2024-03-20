using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Booking : BaseEntity
{
    [Required(ErrorMessage = "User ID is required")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Event ID is required")]
    public Guid EventId { get; set; }

    [Required(ErrorMessage = "Booking date is required")]
    [DataType(DataType.DateTime)]
    public DateTime BookingDate { get; set; }
    
    public User User { get; set; }
    
    public Event Event { get; set; }
}