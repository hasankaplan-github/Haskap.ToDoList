﻿using Haskap.DddBase.Application.UseCaseServices;
using Haskap.DddBase.Utilities.Guids;
using Haskap.ToDoList.Application.UseCaseServices.Contracts;
using Haskap.ToDoList.Application.UseCaseServices.Dtos;
using Haskap.ToDoList.Domain.Core.UserAggregate;
using Haskap.ToDoList.Infrastructure.Data.ToDoListDbContext;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Haskap.ToDoList.Application.UseCaseServices;

public class AccountService : UseCaseService, IAccountService
{
    private readonly AppDbContext _appDbContext;
    //public IConfiguration _configuration;
    private readonly IJwtGenerator _jwtGenerator;


    //private User user = new User(Guid.NewGuid())
    //{
    //    Name = new Name("John", null, "Doe"),
    //    UserName=new UserName("john.doe"),
    //    Password = new Password("123456")
    //};

    public AccountService(AppDbContext appDbContext, /*IConfiguration configuration,*/ IJwtGenerator jwtGenerator)
    {
        _appDbContext=appDbContext;
        //_configuration=configuration;
        _jwtGenerator=jwtGenerator;
    }

    public async Task<LoginOutputDto> LoginAsync(LoginInputDto input)
    {
        var user = _appDbContext.User.SingleOrDefault(x => x.UserName.Value == input.UserName && x.Password.Value == input.Password);

        if (user is null)
        {
            throw new Exception("User not found");
        }

        var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, GuidGenerator.CreateSimpleGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(JwtRegisteredClaimNames.GivenName, user.Name.FirstName),
                        new Claim(JwtRegisteredClaimNames.GivenName + "_2", user.Name.MiddleName ?? string.Empty),
                        new Claim(JwtRegisteredClaimNames.FamilyName, user.Name.LastName),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName.Value),
                    };

        var token = _jwtGenerator.GenerateToken(claims);

        return new LoginOutputDto
        {
            Token=token
        };
    }
}
