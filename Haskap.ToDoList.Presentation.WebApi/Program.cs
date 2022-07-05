using Haskap.ToDoList.Infrastructure.Data.ToDoListDbContext;
using Haskap.ToDoList.Presentation.WebApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Haskap.ToDoList.Application.Dtos.Mappings;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using Haskap.ToDoList.Presentation.WebApi.Middlewares;
using Haskap.ToDoList.Infrastructure.Providers;
using Haskap.ToDoList.Domain.Core;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));

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
            ValidAudience = builder.Configuration[$"{JwtSettings.SectionName}:{nameof(JwtSettings.Audience)}"],
            ValidIssuer = builder.Configuration[$"{JwtSettings.SectionName}:{nameof(JwtSettings.Issuer)}"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[$"{JwtSettings.SectionName}:{nameof(JwtSettings.Secret)}"])),
            ClockSkew = TimeSpan.Zero
        };
    });

var connectionString = builder.Configuration.GetConnectionString("ToDoListConnectionString");
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
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
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>()!;
        var exception = exceptionHandlerPathFeature.Error;

        var exceptionMessage = exception.Message /* is not null ? stringLocalizer[errorMessage] : null */ ;
        var exceptionStackTrace = exception.StackTrace;
        var httpStatusCode = exception switch
        {
            GeneralException generalException => generalException.HttpStatusCode,
            _ => HttpStatusCode.BadRequest
        };
        var errorEnvelope = Envelope.Error(exceptionMessage, exceptionStackTrace, exception.GetType().ToString(), httpStatusCode);

        app.Logger.LogError($"{JsonSerializer.Serialize(errorEnvelope)}{Environment.NewLine}" +
            $"=====================================================================================================================");

        if (app.Environment.IsDevelopment() == false)
        {
            errorEnvelope.SetExceptionStackTraceToNull();
        }

        // using static System.Net.Mime.MediaTypeNames;
        context.Response.ContentType = Text.Plain;
        context.Response.StatusCode = (int)httpStatusCode;
        await context.Response.WriteAsJsonAsync(errorEnvelope);
    });
});



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
    {
        throw new GeneralException("Unauthorized access.", HttpStatusCode.Unauthorized);
    }
});


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
