namespace Infrastructure.Data;

public interface IPaystackRepository
{
    public Task<PayStackResponse> InitializeTransactionAsync(string email, decimal amount);
}