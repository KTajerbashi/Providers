
using Microsoft.OpenApi;
using SwaggerLibrary;

namespace SoapParserWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();

            // Add Swagger with enhanced configuration
            builder.Services.AddSwaggerLibrary(
                new OpenApiInfo
                {
                    Title = "SoapParser API",
                    Version = "v1",
                    Description = "API for parsing SOAP messages and converting them to JSON",
                    Contact = new OpenApiContact
                    {
                        Name = "Kamran Tajerbashi",
                        Email = "kamrantajerbashi@example.com",
                        Url = new Uri("https://github.com/KTajerbashi/Providers")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                },
                configureOptions: options =>
                {
                    // Optional: Add security definitions, filters, etc.
                    // Example: Add JWT Bearer authentication
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Enter 'Bearer' followed by your token"
                    });
                }
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi(); // Keep the built-in OpenAPI endpoint if needed
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Use Swagger UI
            app.UseSwaggerLibrary(
                title: "Soap Parser Web API Documentation",
                version: "v1",
                routePrefix: string.Empty
            );

            app.Run();
        }
    }
}
