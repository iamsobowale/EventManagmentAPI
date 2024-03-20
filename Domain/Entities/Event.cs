using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Event : BaseEntity
{
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public DateTime Date { get; set; }
    
    public string Location { get; set; }
    
    public int TotalTickets { get; set; }
    
    public decimal Price { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; }
}