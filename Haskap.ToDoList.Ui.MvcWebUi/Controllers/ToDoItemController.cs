using Haskap.ToDoList.Application.UseCaseServices.Dtos;
using Haskap.ToDoList.Ui.MvcWebUi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Haskap.ToDoList.Ui.MvcWebUi.Controllers;

public class ToDoItemController : Controller
{
    private readonly ToDoListService _toDoListService;

    public ToDoItemController(ToDoListService toDoListService)
    {
        _toDoListService=toDoListService;
    }

    public async Task<IActionResult> List(Guid toDoListId)
    {
        var items = await _toDoListService.GetToDoItems(toDoListId);
        ViewBag.OwnerToDoListId = toDoListId;
        return View(items);
    }

    [HttpPut]
    public async Task<JsonResult> MarkAsCompleted(MarkAsCompleted_ToDoItemInputDto toDoItemInputDto)
    {
        await _toDoListService.MarkAsCompleted(toDoItemInputDto);
        return Json(new { Success = true });
    }

    [HttpPut]
    public async Task<JsonResult> MarkAsNotCompleted(MarkAsNotCompleted_ToDoItemInputDto toDoItemInputDto)
    {
        await _toDoListService.MarkAsNotCompleted(toDoItemInputDto);
        return Json(new { Success = true });
    }
}
