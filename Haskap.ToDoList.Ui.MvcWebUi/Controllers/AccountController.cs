using Haskap.ToDoList.Application.UseCaseServices.Dtos;
using Haskap.ToDoList.Ui.MvcWebUi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Haskap.ToDoList.Ui.MvcWebUi.Controllers
{
    public class AccountController : Controller
    {
        private readonly ToDoListService _toDoListService;

        public AccountController(ToDoListService toDoListService)
        {
            _toDoListService=toDoListService;
        }
        
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginInputDto loginInputDto)
        {
            var result = await _toDoListService.Login(loginInputDto);
            if (result.HasError)
            {
                return View(result);
            }

            Response.Cookies.Append("jwt", result.Result.Token);
            return LocalRedirect(loginInputDto.ReturnUrl);
        }
    }
}
