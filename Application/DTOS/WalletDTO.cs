namespace Application.DTOS;

public class WalletDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserEmail { get; set; }
    public decimal Balance { get; set; }
}

public class FundWalletDTO
{
    public Guid UserId { get; set; }
    public string UserEmail { get; set; }
    public decimal Amount { get; set; }
    
    public string url { get; set; }
    public string reference { get; set; }
}


