using Microsoft.AspNetCore.Mvc;

namespace Haskap.ToDoList.Presentation.WebApi.Controllers;

public class BaseController : ControllerBase
{
    protected new IActionResult Ok()
    {
        return base.Ok(Envelope.Ok());
    }

    protected IActionResult Ok<T>(T result)
    {
        return base.Ok(Envelope.Ok(result));
    }

    //protected IActionResult Error(string? errorMessage, string? exceptionStackTrace, string? exceptionType)
    //{
    //    return BadRequest(Envelope.Error(errorMessage, exceptionStackTrace, exceptionType));
    //}

    //protected IActionResult FromResult(Result result)
    //{
    //    return result.IsSuccess ? Ok() : Error(result.Error);
    //}
}
