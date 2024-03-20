using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Wallet : BaseEntity
{
    [Required(ErrorMessage = "User ID is required")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Balance is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Balance cannot be negative")]
    public decimal Balance { get; set; }
    public User User { get; set; }
    
    public ICollection<WalletTransaction> WalletTransactions { get; set; }
}