using Haskap.ToDoList.Ui.MvcWebUi.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<ToDoListService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        //var isAjaxRequest = context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        //if (isAjaxRequest)
        //{
        //    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        //    context.Response.ContentType = Text.Plain;
        //    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        //    var exception = exceptionHandlerPathFeature?.Error;
        //    var exceptionMessage = exception?.Message;
        //    await context.Response.WriteAsync(exceptionMessage);
        //}
        //else
        //{
        //    context.Response.Redirect("/Home/Error");
        //}

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;
        if (exception is UnauthorizedAccessException)
        {
            //var controller = context.GetRouteValue("controller")?.ToString();
            //var action = context.GetRouteValue("action")?.ToString();
            context.Response.Redirect($"/Account/Login?returnUrl={context.Request.Path.Value}");
        }
        else if (!app.Environment.IsDevelopment())
        {
            context.Response.Redirect("/Home/Error");
        }
    });
});



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
