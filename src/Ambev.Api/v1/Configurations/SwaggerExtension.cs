using Microsoft.OpenApi.Models;

namespace Ambev_server.v1.Configurations;

public static class SwaggerExtension
{
    public static void AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(type => type.FullName);
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "API de Vendas AMBEV",
                Description = "API para gerenciar vendas AMBEV.",
                Version = "v1"
            });
        });
    }
}