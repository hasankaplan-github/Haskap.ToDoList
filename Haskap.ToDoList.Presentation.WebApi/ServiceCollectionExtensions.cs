using Haskap.ToDoList.Application.UseCaseServices;
using Haskap.ToDoList.Application.UseCaseServices.Contracts;

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
        //services.AddTransient<ICreditCardTypeService, CreditCardTypeService>();
        //services.AddTransient<IAccountService, AccountService>();
        //services.AddTransient<IReportService, ReportService>();
    }

    public static void AddProviders(this IServiceCollection services)
    {
        //services.AddSingleton<LoggedInUserProvider<Guid?>>();
        //services.AddSingleton<VisitIdProvider>();
        //services.AddSingleton<ApplicationIdProvider>();
    }

    public static void AddEfInterceptors(this IServiceCollection services)
    {
        //services.AddScoped<AuditSaveChangesInterceptor<Guid?>>();
        //services.AddScoped<AuditHistoryLogSaveChangesInterceptor<Guid?>>();
    }
}