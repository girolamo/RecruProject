using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecruProject.Console.Logger;
using RecruProject.Core.Logger;
using RecruProject.Core.Repositories;
using RecruProject.Core.Services;
using RecruProject.Core.Validators;
using RecruProject.Infrastructure.Repositories;
using RecruProject.Infrastructure.Services;
using RecruProject.Infrastructure.Validators;

namespace RecruProject.Console.Configuration;

public static class RegisterServices
{
    public static void Register(this IServiceCollection services)
    {

        services.AddSingleton<IConfiguration>(_ => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build());
        services.AddSingleton<IOrderValidator, OrderValidator>();
        services.AddSingleton<ILogger, ConsoleLogger>();
        services.AddSingleton<INotificationService, EmailNotificationService>();
        
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderRepository, OrderRepository>();
    }
}