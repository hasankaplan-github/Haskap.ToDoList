using Haskap.ToDoList.Infrastructure.Data.ToDoListDbContext;
using Haskap.ToDoList.Presentation.WebApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Haskap.ToDoList.Application.UseCaseServices.Dtos.Mappings;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using Haskap.ToDoList.Presentation.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        //options.RequireHttpsMetadata = false;
        //options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"])),
            ClockSkew = TimeSpan.Zero
        };
    });

var connectionString = builder.Configuration.GetConnectionString("ToDoListConnectionString");
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) => {
    options.UseNpgsql(connectionString);
    //options.AddInterceptors(serviceProvider.GetRequiredService<AuditSaveChangesInterceptor<Guid?>>());
    //options.AddInterceptors(serviceProvider.GetRequiredService<AuditHistoryLogSaveChangesInterceptor<Guid?>>());
});

builder.Services.AddProviders();
builder.Services.AddUseCaseServices();

builder.Services.AddAutoMapper(typeof(ToDoListProfile).Assembly);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        // using static System.Net.Mime.MediaTypeNames;
        context.Response.ContentType = Text.Plain;

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

        var exception = exceptionHandlerPathFeature?.Error;

        var exceptionMessage = exception?.Message /* is not null ? stringLocalizer[errorMessage] : null */ ;
        var exceptionStackTrace = exception?.StackTrace;
        var errorEnvelope = Envelope.Error(exceptionMessage, exceptionStackTrace, exception?.GetType().ToString());

        if (exception is UnauthorizedAccessException)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }

        app.Logger.LogError($"{JsonSerializer.Serialize(errorEnvelope)}{ Environment.NewLine}" +
            $"=====================================================================================================================");

        if (app.Environment.IsDevelopment() == false)
        {
            errorEnvelope.ExceptionStackTrace = null;
        }
        await context.Response.WriteAsync(JsonSerializer.Serialize(errorEnvelope));

        //if (exceptionHandlerPathFeature?.Path == "/")
        //{
        //    await context.Response.WriteAsync(" Page: Home.");
        //}
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

/*
app.Use(async (context, next) =>
{
    // Do work that can write to the Response.
    await next.Invoke();
    // Do logging or other work that doesn't write to the Response.
});
*/
app.UseMiddleware<CurrentUserMiddleware>();

app.MapControllers();

app.Run();
