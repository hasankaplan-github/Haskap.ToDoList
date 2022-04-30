using Haskap.ToDoList.Application.UseCaseServices.Contracts;
using Haskap.ToDoList.Application.UseCaseServices.Dtos;
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
    public async Task<IActionResult> Login(LoginInputDto input)
    {
        var output = await _accountService.LoginAsync(input);
        return Ok(output);
    }
}
