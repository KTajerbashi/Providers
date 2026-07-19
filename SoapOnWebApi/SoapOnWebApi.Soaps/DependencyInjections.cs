using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SoapCore;
using SoapOnWebApi.Soaps.Auth;
using SoapOnWebApi.Soaps.Contracts;
using SoapOnWebApi.Soaps.Interfaces;
using SoapOnWebApi.Soaps.Services;

namespace SoapOnWebApi.Soaps;

public static class DependencyInjections
{
    private const string SoapAuthScheme = "BasicAuthentication";

    public static IServiceCollection AddSoapLibrary(this IServiceCollection services)
    {
        services.AddSoapCore();
        services.AddScoped<ISoapUserValidator, SoapUserValidator>();
        services.AddScoped<ICustomerSoapService, CustomerSoapService>();

        // ثبت Scheme به‌صورت کاملاً ایزوله - فقط برای این کتابخونه
        // این خط با AddAuthentication موجود در Program.cs تداخلی ندارد
        services.AddAuthentication()
            .AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(
                SoapAuthScheme, options => { });

        return services;
    }

    public static IApplicationBuilder UseSoapLibrary(this IApplicationBuilder app)
    {
        app.UseWhen(
            context => context.Request.Path.StartsWithSegments("/Services/CustomerService.asmx"),
            branch =>
            {
                branch.Use(async (context, next) =>
                {
                    var result = await context.AuthenticateAsync(SoapAuthScheme);
                    if (!result.Succeeded)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"SoapService\"";
                        return;
                    }
                    context.User = result.Principal!;
                    await next();
                });
            });

        app.UseSoapEndpoint<ICustomerSoapService>(
            path: "/Services/CustomerService.asmx",
            encoder: new SoapEncoderOptions(),
            serializer: SoapSerializer.XmlSerializer,
            caseInsensitivePath: true
        );

        return app;
    }
}