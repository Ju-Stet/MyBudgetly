using System.Reflection;
using MediatR;
using MyBudgetly.Application.Users;
using MyBudgetly.Application.Users.Dto.Mappers;
using MyBudgetly.Domain.Users;
using MyBudgetly.Infrastructure.Users;

namespace MyBudgetly.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("MyBudgetly.Application")));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        // dbo mappers
        services.AddSingleton<UserDboMapper>();

        // dto mappers
        services.AddScoped<UserDtoMapper>();

        // services
        services.AddScoped<UserDomainService>();
        services.AddScoped<UserApplicationService>();

        // validators
        services.AddScoped<IUserUniquenessChecker, UserUniquenessChecker>();

        return services;
    }
}