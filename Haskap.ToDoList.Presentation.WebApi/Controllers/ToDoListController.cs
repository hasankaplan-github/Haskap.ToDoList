using Haskap.ToDoList.Application.UseCaseServices.Contracts;
using Haskap.ToDoList.Application.UseCaseServices.Dtos;
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
    private readonly Guid _ownerUserId;

    public ToDoListController(IToDoListService toDoListService)
    {
        _toDoListService=toDoListService;
        _ownerUserId = Guid.Parse(User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)!.Value);        
    }

    [Authorize]
    [HttpPost("AddToDoList")]
    public async Task<IActionResult> AddToDoList([FromBody]string name)
    {
        await _toDoListService.AddToDoList(_ownerUserId, name);
        return Ok();
    }

    [Authorize]
    [HttpPost("AddToDoItem")]
    public async Task<IActionResult> AddToDoItem(Guid toDoListId, ToDoItemInputDto toDoItemInputDto)
    {
        await _toDoListService.AddToDoItem(_ownerUserId, toDoListId, toDoItemInputDto);
        return Ok();
    }

    [Authorize]
    [HttpPost("DeleteToDoList")]
    public async Task<IActionResult> DeleteToDoList(Guid toDoListId)
    {
        await _toDoListService.DeleteToDoList(_ownerUserId, toDoListId);
        return Ok();
    }

    [Authorize]
    [HttpPost("UpdateToDoList")]
    public async Task<IActionResult> UpdateToDoList(Guid toDoListId, string name)
    {
        await _toDoListService.UpdateToDoList(_ownerUserId, toDoListId, name);
        return Ok();
    }

    [Authorize]
    [HttpPost("DeleteToDoItem")]
    public async Task<IActionResult> DeleteToDoItem(Guid toDoListId, Guid toDoItemId)
    {
        await _toDoListService.DeleteToDoItem(_ownerUserId, toDoListId, toDoItemId);
        return Ok();
    }

    [Authorize]
    [HttpPost("UpdateToDoItem")]
    public async Task<IActionResult> UpdateToDoItem(Guid toDoListId, Guid toDoItemId, ToDoItemInputDto toDoItemInputDto)
    {
        await _toDoListService.UpdateToDoItem(_ownerUserId, toDoListId, toDoItemId, toDoItemInputDto);
        return Ok();
    }

    [Authorize]
    [HttpPost("GetToDoLists")]
    public async Task<IActionResult> GetToDoLists()
    {
        var outputDto = await _toDoListService.GetToDoLists(_ownerUserId);
        return Ok(outputDto);
    }

    [Authorize]
    [HttpPost("GetToDoItems")]
    public async Task<IActionResult> GetToDoItems(Guid toDoListId)
    {
        var outputDto = await _toDoListService.GetToDoItems(_ownerUserId, toDoListId);
        return Ok(outputDto);
    }
}
