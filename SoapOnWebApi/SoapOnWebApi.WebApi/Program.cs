using Microsoft.AspNetCore.Authentication.JwtBearer;
using SoapCore;
using SoapOnWebApi.Soaps;
using SoapOnWebApi.Soaps.Contracts;
using SoapOnWebApi.Soaps.Services;
using SwaggerLibrary;

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

            // --- Authentication اصلی پروژه (JWT) - دست‌نخورده باقی می‌مونه ---
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => { /* تنظیمات JWT شما */ });


            builder.Services.AddSoapLibrary();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); // مهم: قبل از UseSoapLibrary باشه
            
            app.UseAuthorization();

            app.UseSwaggerLibrary("WebApi", routePrefix: string.Empty);

            app.MapControllers();
            
            app.UseSoapLibrary();

            app.Run();
        }
    }
}
