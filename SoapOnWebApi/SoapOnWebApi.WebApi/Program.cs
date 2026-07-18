
using SwaggerLibrary;
using SoapOnWebApi.Soaps;

namespace SoapOnWebApi.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddSwaggerLibrary(new Microsoft.OpenApi.OpenApiInfo()
            {
                Title = "WebApi",
                Version = "v1",
            }, opt =>
            {
            });

            builder.Services.AddSoapLibrary();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseSoapLibrary();

            app.UseSwaggerLibrary("WebApi", routePrefix: string.Empty);

            app.MapControllers();

            app.Run();
        }
    }
}
