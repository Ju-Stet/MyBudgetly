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
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "MyBudgetly API",
                Version = "v1",
                Description = "A simple API for managing personal budgets",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Name = "Julia Stetsenko",
                    Email = "jul.stetsenko@gmail.com",
                    Url = new Uri("https://github.com/Ju-Stet/MyBudgetly")
                }
            });
        });

        var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.Run();
        }
}