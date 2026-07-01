using Microsoft.EntityFrameworkCore;
using SwaggerLibrary;
using UnitOfWork.WebApp.Components;
using UnitOfWork.WebApp.Infrastructure;
using UnitOfWork.WebApp.Provider;

namespace UnitOfWork.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddControllers();

            builder.Services.AddAggregateRepository();

            builder.Services.AddSwaggerLibrary(new()
            {
                Title = "Unit Of Work",
                Version = "v1",
                Description = "Unit Of Work Api",
                Summary = "UNIT_OF_WORK",
                License = new() { Name = "MIT", Url = new("https://google.com"), Identifier = "" },
                Contact = new() { Name = "Google", Email = "google@gamil.com", Url = new("https://google.com") },
            }, config =>
            {

            });

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwaggerLibrary("Unit Of Work");

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();

            app.MapControllers();   // <-- Add this

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
