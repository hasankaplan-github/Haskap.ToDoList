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
        
        public async Task<IActionResult> Login(string returnUrl)
        {
            //var result = await _toDoListService.Login(new LoginInputDto());
            
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginInputDto loginInputDto)
        {
            //await _toDoListService.AddToDoList(new ToDoListInputDto());
            var result = await _toDoListService.Login(loginInputDto);
            
            if (result.HasError)
            {
                ViewBag.ReturnUrl = loginInputDto.ReturnUrl;
                return View(result);
            }

            var cookieOptions = new CookieOptions { SameSite = SameSiteMode.Strict };
            if (loginInputDto.RememberMe)
                cookieOptions.Expires = DateTime.Now.AddDays(7);
            else
                cookieOptions.Expires = DateTime.Now.AddHours(1);
            Response.Cookies.Append("jwt", result.Result!.Token, cookieOptions);
            
            loginInputDto.ReturnUrl = string.IsNullOrEmpty(loginInputDto.ReturnUrl) ? "/ToDoList/List" : loginInputDto.ReturnUrl;
            return LocalRedirect(loginInputDto.ReturnUrl);
        }
    }
}
