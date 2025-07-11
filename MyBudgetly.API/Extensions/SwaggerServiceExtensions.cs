public static class SwaggerServiceExtensions
{
    public static IServiceCollection AddSwaggerDocs(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
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

        return services;
    }
}