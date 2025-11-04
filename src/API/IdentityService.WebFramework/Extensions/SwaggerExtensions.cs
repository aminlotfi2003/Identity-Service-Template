using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace IdentityService.WebFramework.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerConfigured(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Identity Service API",
                Version = "v1",
                Description = "ASP.NET Core Identity Service (CQRS, Clean Architecture, Versioned API)"
            });

            opt.AddSecurityDefinition("cookieAuth", new OpenApiSecurityScheme
            {
                Name = ".ids.auth",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Cookie,
                Description = "Cookie-based authentication"
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "cookieAuth" } },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerConfigured(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {
        app.UseSwagger();
        app.UseSwaggerUI(opt =>
        {
            foreach (var desc in provider.ApiVersionDescriptions)
            {
                opt.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
            }
            opt.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            opt.RoutePrefix = "swagger";
        });

        return app;
    }
}
