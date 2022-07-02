using Haskap.ToDoList.Application.UseCaseServices.Dtos;
using Haskap.ToDoList.Ui.MvcWebUi.HttpClients;
using Microsoft.AspNetCore.Mvc;

namespace Haskap.ToDoList.Ui.MvcWebUi.Controllers;

public class ToDoItemController : Controller
{
    private readonly ToDoListHttpClient _toDoListHttpClient;

    public ToDoItemController(ToDoListHttpClient toDoListHttpClient)
    {
        _toDoListHttpClient=toDoListHttpClient;
    }

    public async Task<IActionResult> List(Guid toDoListId)
    {
        var items = await _toDoListHttpClient.GetToDoItems(toDoListId);
        ViewBag.OwnerToDoListId = toDoListId;
        return View(items);
    }

    [HttpPut]
    public async Task<JsonResult> MarkAsCompleted(MarkAsCompleted_ToDoItemInputDto toDoItemInputDto)
    {
        await _toDoListHttpClient.MarkAsCompleted(toDoItemInputDto);
        return Json(new { Success = true });
    }

    [HttpPut]
    public async Task<JsonResult> MarkAsNotCompleted(MarkAsNotCompleted_ToDoItemInputDto toDoItemInputDto)
    {
        await _toDoListHttpClient.MarkAsNotCompleted(toDoItemInputDto);
        return Json(new { Success = true });
    }
}
