using Haskap.DddBase.Application.UseCaseServices;
using Haskap.ToDoList.Application.Contracts;
using Haskap.ToDoList.Application.Dtos;
using Haskap.ToDoList.Domain.Core;
using Haskap.ToDoList.Domain.Core.UserAggregate;
using Haskap.ToDoList.Domain.Core.UserAggregate.Exceptions;
using Haskap.ToDoList.Domain.Providers;
using Haskap.ToDoList.Infrastructure.Data.ToDoListDbContext;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Haskap.ToDoList.Application.UseCaseServices;

public class AccountService : UseCaseService, IAccountService
{
    private readonly AppDbContext _appDbContext;
    //public IConfiguration _configuration;
    private readonly IJwtProvider _jwtProvider;


    //private User user = new User(Guid.NewGuid())
    //{
    //    Name = new Name("John", null, "Doe"),
    //    UserName=new UserName("john.doe"),
    //    Password = new Password("123456")
    //};

    public AccountService(AppDbContext appDbContext, /*IConfiguration configuration,*/ IJwtProvider jwtProvider)
    {
        _appDbContext=appDbContext;
        //_configuration=configuration;
        _jwtProvider=jwtProvider;
    }

    public async Task<LoginOutputDto> LoginAsync(LoginInputDto loginInputDto)
    {
        var user = _appDbContext.User.SingleOrDefault(x => x.UserName.Value == loginInputDto.UserName && x.Password.Value == loginInputDto.Password);

        if (user is null)
        {
            throw new UserNotFoundException();
        }

        var token = _jwtProvider.GenerateToken(user);

        return new LoginOutputDto
        {
            Token=token
        };
    }
}
