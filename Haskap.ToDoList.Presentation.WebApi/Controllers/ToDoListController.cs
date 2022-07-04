using Haskap.ToDoList.Application.UseCaseServices.Contracts;
using Haskap.ToDoList.Application.UseCaseServices.Dtos;
using Haskap.ToDoList.Application.UseCaseServices.Dtos.DataTable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Haskap.ToDoList.Presentation.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ToDoListController : BaseController
{
    private readonly IToDoListService _toDoListService;

    public ToDoListController(IToDoListService toDoListService)
    {
        _toDoListService=toDoListService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddToDoList(ToDoListInputDto toDoListInputDto)
    {
        await _toDoListService.AddToDoList(toDoListInputDto);
        return Ok();
    }

    [Authorize]
    [HttpDelete("{toDoListId}")]
    public async Task<IActionResult> DeleteToDoList(Guid toDoListId)
    {
        await _toDoListService.DeleteToDoList(toDoListId);
        return Ok();
    }

    [Authorize]
    [HttpPut("{toDoListId}")]
    public async Task<IActionResult> UpdateToDoList(Guid toDoListId, ToDoListInputDto toDoListInputDto)
    {
        await _toDoListService.UpdateToDoList(toDoListId, toDoListInputDto);
        return Ok();
    }

    [Authorize]
    [HttpPost("List")]
    public async Task<IActionResult> GetToDoLists(JqueryDataTableParam jqueryDataTableParam)
    {
        var outputDto = await _toDoListService.GetToDoLists(jqueryDataTableParam);
        return Ok(outputDto);
    }

    [Authorize]
    [HttpPost("MarkAsCompleted")]
    public async Task<IActionResult> MarkAsCompleted(MarkAsCompleted_ToDoListInputDto toDoListInputDto)
    {
        await _toDoListService.MarkToDoListAsCompleted(toDoListInputDto);
        return Ok();
    }


}
