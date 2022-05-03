using Haskap.ToDoList.Application.UseCaseServices.Dtos;
using Haskap.ToDoList.Ui.MvcWebUi.Models;
using Haskap.ToDoList.Ui.MvcWebUi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Haskap.ToDoList.Ui.MvcWebUi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ToDoListService _toDoListService;

        public HomeController(ILogger<HomeController> logger, ToDoListService toDoListService)
        {
            _logger = logger;
            _toDoListService=toDoListService;
        }

        public async Task<IActionResult> Index()
        {
            var loginResult = await _toDoListService.Login(new LoginInputDto { Password= "123", UserName = "admin" });
            if (loginResult.HasError == false)
            {
                Response.Cookies.Append("jwtToken", loginResult.Result.Token);
            }
            
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var lists = await _toDoListService.GetToDoLists();            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}