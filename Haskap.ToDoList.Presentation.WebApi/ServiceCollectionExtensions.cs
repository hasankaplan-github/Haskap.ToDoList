using Haskap.DddBase.Domain.Providers;
using Haskap.ToDoList.Application.UseCaseServices;
using Haskap.ToDoList.Application.UseCaseServices.Contracts;
using Haskap.ToDoList.Domain.Core.UserAggregate;
using Haskap.ToDoList.Domain.Providers;
using Haskap.ToDoList.Infrastructure.Jwt;
using Haskap.ToDoList.Infrastructure.Providers;

namespace Haskap.ToDoList.Presentation.WebApi;

public static class ServiceCollectionExtensions
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        //services.AddTransient<OrderIdDomainService>();
        //services.AddTransient<PaymentCredentialsDomainService>();
    }

    public static void AddUseCaseServices(this IServiceCollection services)
    {
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IToDoListService, ToDoListService>();
        services.AddTransient<IToDoItemService, ToDoItemService>();
        //services.AddTransient<ICreditCardTypeService, CreditCardTypeService>();
        //services.AddTransient<IAccountService, AccountService>();
        //services.AddTransient<IReportService, ReportService>();
    }

    public static void AddProviders(this IServiceCollection services)
    {
        services.AddScoped<CurrentUserProvider<User, Guid>>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IJwtGenerator, JwtGenerator>();
        //services.AddSingleton<VisitIdProvider>();
        //services.AddSingleton<ApplicationIdProvider>();
    }

    public static void AddEfInterceptors(this IServiceCollection services)
    {
        //services.AddScoped<AuditSaveChangesInterceptor<Guid?>>();
        //services.AddScoped<AuditHistoryLogSaveChangesInterceptor<Guid?>>();
    }
}