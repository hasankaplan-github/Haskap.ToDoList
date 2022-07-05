using Haskap.ToDoList.Application.Contracts;
using Haskap.ToDoList.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Haskap.ToDoList.Presentation.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ToDoItemController : BaseController
{
    private readonly IToDoItemService _toDotemService;

    public ToDoItemController(IToDoItemService toDoItemService)
    {
        _toDotemService=toDoItemService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddToDoItem(ToDoItemInputDto toDoItemInputDto)
    {
        await _toDotemService.AddToDoItem(toDoItemInputDto);
        return Ok();
    }

    [Authorize]
    [HttpDelete("{ownerToDoListId}/{toDoItemId}")]
    public async Task<IActionResult> DeleteToDoItem(Guid ownerToDoListId, Guid toDoItemId)
    {
        await _toDotemService.DeleteToDoItem(ownerToDoListId, toDoItemId);
        return Ok();
    }

    [Authorize]
    [HttpPut("{toDoItemId}")]
    public async Task<IActionResult> UpdateToDoItem(Guid toDoItemId, ToDoItemInputDto toDoItemInputDto)
    {
        await _toDotemService.UpdateToDoItem(toDoItemId, toDoItemInputDto);
        return Ok();
    }

    [Authorize]
    [HttpGet("List/{ownerToDoListId}")]
    public async Task<IActionResult> GetToDoItems(Guid ownerToDoListId)
    {
        var outputDto = await _toDotemService.GetToDoItems(ownerToDoListId);
        return Ok(outputDto);
    }

    [Authorize]
    [HttpPost("MarkAsCompleted")]
    public async Task<IActionResult> MarkAsCompleted(MarkAsCompleted_ToDoItemInputDto toDoItemInputDto)
    {
        await _toDotemService.MarkToDoItemAsCompleted(toDoItemInputDto);
        return Ok();
    }

    [Authorize]
    [HttpPost("MarkAsNotCompleted")]
    public async Task<IActionResult> MarkAsNotCompleted(MarkAsNotCompleted_ToDoItemInputDto toDoItemInputDto)
    {
        await _toDotemService.MarkToDoItemAsNotCompleted(toDoItemInputDto);
        return Ok();
    }
}
