
using System.IdentityModel.Tokens.Jwt;
using Domain.Entities;

namespace Application.Identity;

public interface IIdentityService
{
    string GetUserIdentity();

    string GenerateToken(User user);

    public JwtSecurityToken GetClaims(string token);

}