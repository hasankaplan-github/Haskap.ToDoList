using Haskap.ToDoList.Application.UseCaseServices.Contracts;
using Haskap.ToDoList.Domain.Providers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Haskap.ToDoList.Infrastructure.Jwt;
public class JwtGenerator : IJwtGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;

    public JwtGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtSettingsOptions)
    {
        _dateTimeProvider=dateTimeProvider;
        _jwtSettings=jwtSettingsOptions.Value;
    }

    public string GenerateToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
