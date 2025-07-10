using Microsoft.EntityFrameworkCore;
using MyBudgetly.API.Extensions;
using MyBudgetly.Infrastructure.Extensions;
using MyBudgetly.Infrastructure.Persistence;
using MyBudgetly.Infrastructure.Persistence.Abstractions;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services
        builder.Services.AddSwaggerDocs();
        builder.Services.AddControllers();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IApplicationDbContext>(
            provider => provider.GetRequiredService<ApplicationDbContext>());
        builder.Services.AddInfrastructure();
        builder.Services.AddApplication();        

        var app = builder.Build();

        // Middleware
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseHttpsRedirection();
        app.MapControllers();
        app.Run();
    }
}