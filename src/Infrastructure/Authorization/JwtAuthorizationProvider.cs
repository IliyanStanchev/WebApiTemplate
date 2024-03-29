using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApiTemplate.Application.Interfaces;
using WebApiTemplate.Domain.Options;

namespace WebApiTemplate.Infrastructure.Authorization;

public class JwtAuthorizationProvider( IOptions<AuthenticationOptions> options ) : IAuthorizationProvider
{
    public async Task<string> GenerateTokenAsync()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(options.Value.Issuer,
                                            options.Value.Issuer,
                                            null,
                                            expires: DateTime.Now.AddMinutes(120),
                                            signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return await Task.FromResult(token);
    }
}
