using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Gender { get; set; }
    public Wallet Wallet { get; set; }
    public string Password { get; set; }
    public virtual ICollection<Booking> Bookings { get; set; }
}