using Haskap.ToDoList.Application.UseCaseServices.Dtos;
using Haskap.ToDoList.Ui.MvcWebUi.HttpClients;
using Microsoft.AspNetCore.Mvc;

namespace Haskap.ToDoList.Ui.MvcWebUi.Controllers;

public class ToDoListController : Controller
{
    private readonly ToDoListHttpClient _toDoListHttpClient;

    public ToDoListController(ToDoListHttpClient toDoListHttpClient)
    {
        _toDoListHttpClient=toDoListHttpClient;
    }

    public async Task<IActionResult> List()
    {
        var lists = await _toDoListHttpClient.GetToDoLists<IEnumerable<ToDoListOutputDto>>();
        return View(lists);
    }

    [HttpPost]
    public async Task<JsonResult> AddToDoList(ToDoListInputDto toDoListInputDto)
    {
        var result = await _toDoListHttpClient.AddToDoList(toDoListInputDto);

        return Json(result);
    }
}
