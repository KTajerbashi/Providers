
using CT.WebApi.Repositories;
using CT.WebApi.Services;
using SwaggerLibrary;
using System.Reflection;

namespace CT.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddScoped<IUserRepository, UserService>();
            builder.Services.AddMediatR((x) =>
            {
                x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerLibrary(new()
            {
                Title = "Cancellation Token Apis",
                Version = "v1",
                Description = "Cancellation Token Api",
                Summary = "Cancellation_Token",
                License = new() { Name = "MIT", Url = new("https://google.com"), Identifier = "" },
                Contact = new() { Name = "Google", Email = "google@gamil.com", Url = new("https://google.com") },
            }, config =>
            {

            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerLibrary("Cancellation Token");

            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
