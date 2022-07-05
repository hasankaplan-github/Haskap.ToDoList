using Haskap.ToDoList.Application.Dtos;
using Haskap.ToDoList.Application.Dtos.DataTable;
using Haskap.ToDoList.Ui.MvcWebUi.HttpClients;
using Haskap.ToDoList.Ui.MvcWebUi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Haskap.ToDoList.Ui.MvcWebUi.Controllers;

public class ToDoListController : Controller
{
    private readonly ToDoListHttpClient _toDoListHttpClient;

    public ToDoListController(ToDoListHttpClient toDoListHttpClient)
    {
        _toDoListHttpClient=toDoListHttpClient;
    }

    public async Task<IActionResult> List(int pageSize = 2, int currentPageIndex = 1)
    {
        JqueryDataTableParam jqueryDataTableParam = new()
        {
            Length = pageSize,
            Start = (currentPageIndex - 1) * pageSize
        };
        var lists = await _toDoListHttpClient.GetToDoLists(jqueryDataTableParam);
        
        var pagination = new Pagination(pageSize, currentPageIndex, lists.Result?.recordsFiltered ?? 0);
        ViewBag.Pagination = pagination;

        return View(lists);
    }

    [HttpPost]
    public async Task<JsonResult> AddToDoList(ToDoListInputDto toDoListInputDto)
    {
        var result = await _toDoListHttpClient.AddToDoList(toDoListInputDto);

        return Json(result);
    }
}
