using Haskap.ToDoList.Application.Dtos;
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
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginInputDto loginInputDto)
        {
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
            
            return LocalRedirect(string.IsNullOrWhiteSpace(loginInputDto.ReturnUrl) ? "/ToDoList/List" : loginInputDto.ReturnUrl);
        }
    }
}
