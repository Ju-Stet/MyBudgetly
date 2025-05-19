using Microsoft.Extensions.DependencyInjection;
using MyBudgetly.Domain.Users;
using MyBudgetly.Infrastructure.Users;

namespace MyBudgetly.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserUniquenessChecker, UserUniquenessChecker>();

        return services;
    }
}