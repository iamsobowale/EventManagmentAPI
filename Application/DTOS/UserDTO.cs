using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.DTOS;

public class UserDTO
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string Gender { get; set; }
    public WalletDTO Wallet { get; set; }
}

public class CreateUserResponseDTO
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
    public string Gender { get; set; }
}
