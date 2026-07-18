using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SoapOnWebApi.Soaps.Contracts;
using SoapOnWebApi.Soaps.Services;

namespace SoapOnWebApi.Soaps;

public static class DependencyInjections
{
    public static IServiceCollection AddSoapLibrary(this IServiceCollection services)
    {
        services.AddScoped<ICalculatorRepository, CalculatorRepository>();
        return services;
    }
    public static IApplicationBuilder UseSoapLibrary(this IApplicationBuilder app)
    {
        return app;
    }
}
