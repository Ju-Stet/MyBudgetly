using Microsoft.EntityFrameworkCore;
using MyBudgetly.Application.Interfaces;
using MyBudgetly.Infrastructure.Extensions;
using MyBudgetly.Infrastructure.Persistence;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped(provider => provider.GetRequiredService<IApplicationDbContext>());
        builder.Services.AddInfrastructure();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.Run();
    }
}