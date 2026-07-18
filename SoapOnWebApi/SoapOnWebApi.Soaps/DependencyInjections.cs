using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SoapOnWebApi.Soaps;

public static class DependencyInjections
{
    public static IServiceCollection AddSoapLibrary(this IServiceCollection services)
    {
        return services;
    }
    public static IApplicationBuilder UseSoapLibrary(this IApplicationBuilder app)
    {
        return app;
    }
}
