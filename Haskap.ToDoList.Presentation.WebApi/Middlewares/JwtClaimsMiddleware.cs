using Haskap.ToDoList.Domain.Providers;
using System.Security.Claims;

namespace Haskap.ToDoList.Presentation.WebApi.Middlewares;

public class JwtClaimsMiddleware
{
    private readonly RequestDelegate _next;

    public JwtClaimsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, JwtClaimsProvider jwtClaimsProvider)
    {
        jwtClaimsProvider.LoggedInUserId = Guid.Parse(httpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        await _next(httpContext);
    }
}
