namespace Application.DTOS;

public class WalletTransactionDTO
{
    public Guid Id { get; set; }
    public int WalletTransactionId { get; set; }
    public Guid WalletId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Description { get; set; }
    public string TransactionType { get; set; }
}