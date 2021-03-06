using Haskap.ToDoList.Application.Contracts;
using Haskap.ToDoList.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Haskap.ToDoList.Presentation.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : BaseController
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService=accountService;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginInputDto loginInputDto)
    {
        var loginOutputDto = await _accountService.LoginAsync(loginInputDto);
        return Ok(loginOutputDto);
    }
}
