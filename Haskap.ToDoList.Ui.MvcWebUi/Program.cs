using Haskap.ToDoList.Ui.MvcWebUi;
using Haskap.ToDoList.Ui.MvcWebUi.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Net.Http.Headers;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<ToDoListService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["ToDoListApiBaseUrl"]);
    httpClient.DefaultRequestHeaders.Clear();
    httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "*/*");
});
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
            var path = context.Request.Path.HasValue ? context.Request.Path.Value : string.Empty;
            var queryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : string.Empty;
            context.Response.Redirect($"/Account/Login?returnUrl={path}{queryString}");
        }
        else //if (!app.Environment.IsDevelopment())
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
