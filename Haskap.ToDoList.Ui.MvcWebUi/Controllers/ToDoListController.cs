using Haskap.ToDoList.Ui.MvcWebUi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Haskap.ToDoList.Ui.MvcWebUi.Controllers
{
    public class ToDoListController : Controller
    {
        private readonly ToDoListService _toDoListService;

        public ToDoListController(ToDoListService toDoListService)
        {
            _toDoListService=toDoListService;
        }

        public async Task<IActionResult> List()
        {
            var lists = await _toDoListService.GetToDoLists();
            return View(lists);
        }
    }
}
