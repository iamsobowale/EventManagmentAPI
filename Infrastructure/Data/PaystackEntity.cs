namespace Infrastructure.Data;

public class PaystackEntity
{
    public string Id { get; set; }
    public string Reference { get; set; }
}

public class TransactionInitialize
{
    public string authorization_url { get; set; }
    public string reference { get; set; }
}

public class PayStackResponse
{
    public bool status { get; set; }
    public string message { get; set; }
    public TransactionInitialize data { get; set; }
}