using Haskap.ToDoList.Application.UseCaseServices.Dtos;
using Haskap.ToDoList.Ui.MvcWebUi.HttpClients;
using Microsoft.AspNetCore.Mvc;

namespace Haskap.ToDoList.Ui.MvcWebUi.Controllers
{
    public class AccountController : Controller
    {
        private readonly ToDoListHttpClient _toDoListHttpClient;

        public AccountController(ToDoListHttpClient toDoListHttpClient)
        {
            _toDoListHttpClient=toDoListHttpClient;
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
            var envelope = await _toDoListHttpClient.Login(loginInputDto);
            
            if (envelope.HasError)
            {
                ViewBag.ReturnUrl = loginInputDto.ReturnUrl;
                return View(envelope);
            }

            var cookieOptions = new CookieOptions { SameSite = SameSiteMode.Strict };
            if (loginInputDto.RememberMe)
                cookieOptions.Expires = DateTime.Now.AddDays(7);
            else
                cookieOptions.Expires = DateTime.Now.AddHours(1);
            Response.Cookies.Append("jwt", envelope.Result!.Token, cookieOptions);
            
            loginInputDto.ReturnUrl = string.IsNullOrEmpty(loginInputDto.ReturnUrl) ? "/ToDoList/List" : loginInputDto.ReturnUrl;
            return LocalRedirect(loginInputDto.ReturnUrl);
        }
    }
}
