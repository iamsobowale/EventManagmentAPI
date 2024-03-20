using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Identity;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly IConfiguration _configuration;
    
    public IdentityService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GetUserIdentity()
    {
        return "_context.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value;";
    }

    public string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtTokenSettings:TokenKey").Value));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        IList<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Sid, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };
        
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public JwtSecurityToken GetClaims(string token)
    {
        if (!string.IsNullOrEmpty(token))
        {
                
            var handler = new JwtSecurityTokenHandler();

            var decodedToken = handler.ReadToken(token) as JwtSecurityToken;

            return decodedToken;
        }
        return null;
    }
}