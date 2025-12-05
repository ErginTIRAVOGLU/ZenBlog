using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Application.Features.Users;
using ZenBlog.Application.Options;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Persistence.Concrete;

public class JwtService(
    UserManager<AppUser> userManager,
    IOptions<JwtTokenOptions> jwtOptions
    ) : IJwtService
{
    public async Task<GetLoginQueryResult> GenerateTokenAsync(UserCreateResult user)
    {

        var UserLoginInfo = await userManager.FindByEmailAsync(user.Email);

        if(UserLoginInfo == null)
        {
            UserLoginInfo= await userManager.FindByNameAsync(user.UserName);
            if (UserLoginInfo is null)
            {
                throw new Exception("User not found");
            }
        }

       SymmetricSecurityKey key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey));

        var dateTimeNow = DateTime.UtcNow;

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, UserLoginInfo.Id),
            new Claim(JwtRegisteredClaimNames.Name, UserLoginInfo.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, UserLoginInfo.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("fullname", $"{UserLoginInfo.FirstName} {UserLoginInfo.LastName}")
        };

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: jwtOptions.Value.Issuer,
            audience: jwtOptions.Value.Audience,
            claims: claims,
            notBefore: dateTimeNow,
            expires: dateTimeNow.AddDays(jwtOptions.Value.ExpirationInDays),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        GetLoginQueryResult response = new()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = dateTimeNow.AddDays(jwtOptions.Value.ExpirationInDays)
        };

        return response;
    }
}
