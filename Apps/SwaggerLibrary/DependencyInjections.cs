using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SwaggerLibrary;

public static class DependencyInjections
{
    /// <summary>
    /// Adds Swagger generation services to the container
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to</param>
    /// <param name="apiInfo">API information for the Swagger document</param>
    /// <param name="configureOptions">Optional additional configuration for SwaggerGen</param>
    /// <returns>The modified IServiceCollection</returns>
    public static IServiceCollection AddSwaggerLibrary(
        this IServiceCollection services,
        OpenApiInfo apiInfo,
        Action<SwaggerGenOptions>? configureOptions = null)
    {
        // Add Swagger generation
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(apiInfo.Version ?? "v1", apiInfo);

            // Optional: Include XML comments if you have them
            var xmlFile = $"{System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }

            // Apply any additional configuration
            configureOptions?.Invoke(c);
        });

        return services;
    }

    /// <summary>
    /// Adds Swagger middleware to the application pipeline
    /// </summary>
    /// <param name="app">The WebApplication instance</param>
    /// <param name="title">Title to display in Swagger UI</param>
    /// <param name="version">API version (defaults to "v1")</param>
    /// <param name="routePrefix">Route prefix for Swagger UI (defaults to "swagger")</param>
    /// <param name="configureOptions">Optional additional configuration for SwaggerUI</param>
    /// <returns>The modified WebApplication</returns>
    public static WebApplication UseSwaggerLibrary(
        this WebApplication app,
        string title,
        string version = "v1",
        string routePrefix = "swagger",
        Action<SwaggerUIOptions>? configureOptions = null)
    {
        // Only enable Swagger in development environment by default
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{version}/swagger.json", title);
                c.RoutePrefix = routePrefix;

                // Optional: Customize UI
                c.DocumentTitle = title;
                c.DefaultModelsExpandDepth(-1); // Hide models section

                // Apply any additional configuration
                configureOptions?.Invoke(c);
            });
        }

        return app;
    }

    /// <summary>
    /// Enhanced version that allows enabling Swagger in non-development environments
    /// </summary>
    public static WebApplication UseSwaggerLibraryWithEnvironmentCheck(
        this WebApplication app,
        string title,
        string version = "v1",
        bool enableInProduction = false,
        string routePrefix = "swagger")
    {
        if (app.Environment.IsDevelopment() || enableInProduction)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{version}/swagger.json", title);
                c.RoutePrefix = routePrefix;
            });
        }

        return app;
    }
}
