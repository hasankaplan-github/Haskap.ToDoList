using Haskap.ToDoList.Domain.Core.UserAggregate;
using System.Security.Claims;
using Haskap.DddBase.Domain.Providers;

namespace Haskap.ToDoList.Presentation.WebApi.Middlewares;

public class CurrentUserMiddleware
{
    private readonly RequestDelegate _next;

    public CurrentUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, CurrentUserProvider<User, Guid> currentUserProvider)
    {
        if (httpContext.User.Identity.IsAuthenticated)
        {
            var currentUser = new User(Guid.Parse(httpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value));
            currentUser.Name = new Name(
                httpContext.User.FindFirst("FirstName")!.Value,
                httpContext.User.FindFirst("MiddleName")?.Value,
                httpContext.User.FindFirst("LastName")!.Value);
            currentUser.UserName = new UserName(httpContext.User.FindFirst("UserName")!.Value);

            currentUserProvider.User = currentUser;
        }
        
        await _next(httpContext);
    }
}
