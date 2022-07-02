using Haskap.DddBase.Utilities.Guids;
using Haskap.ToDoList.Domain.Core.UserAggregate;
using Haskap.ToDoList.Domain.Providers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Haskap.ToDoList.Infrastructure.Providers;
public class JwtProvider : IJwtProvider
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;

    public JwtProvider(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtSettingsOptions)
    {
        _dateTimeProvider=dateTimeProvider;
        _jwtSettings=jwtSettingsOptions.Value;
    }

    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, GuidGenerator.CreateSimpleGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(JwtRegisteredClaimNames.GivenName, user.Name.FirstName),
                        new Claim(JwtRegisteredClaimNames.GivenName + "_2", user.Name.MiddleName ?? string.Empty),
                        new Claim(JwtRegisteredClaimNames.FamilyName, user.Name.LastName),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName.Value),
                    };

        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
