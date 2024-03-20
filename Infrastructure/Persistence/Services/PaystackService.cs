using System.Net.Http.Headers;
using System.Text;
using Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Infrastructure.Persistence.Services;

public class PaystackService : IPaystackRepository
{
    private readonly IConfiguration _configuration;
    
    public PaystackService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<PayStackResponse> InitializeTransactionAsync(string email, decimal amount)
    {
        
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.BaseAddress = new Uri("https://api.paystack.co/transaction/initialize");
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _configuration.GetSection("PaystackSettings:SecretKey").Value);
        var content = new StringContent(JsonConvert.SerializeObject(new
        {
            amount = amount * 100,
            email = email,
            reference = Guid.NewGuid().ToString(),
            metadata = new
            {
                transaction_id = Guid.NewGuid().ToString(),
            }
        }), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("https://api.paystack.co/transaction/initialize", content);
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<PayStackResponse>(responseString);
        return responseObject;
    }
}