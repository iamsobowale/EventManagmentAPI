using System.ComponentModel.DataAnnotations;

namespace Application.DTOS;

public class LoginDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginResponseDTO
{
    public string Token { get; set; }
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
}