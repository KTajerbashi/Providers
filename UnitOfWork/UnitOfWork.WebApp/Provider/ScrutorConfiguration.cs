using Scrutor;
using System.Reflection;
using UnitOfWork.WebApp.Common;

namespace UnitOfWork.WebApp.Provider;

public interface IScoped { }
public interface ITransient { }
public interface ISingleton { }
public static class ScrutorConfiguration
{

    public static IServiceCollection AddAggregateRepository(this IServiceCollection services)
    {
        //services.Scan(scan => scan
        //    .FromAssemblies(Assembly.GetExecutingAssembly())
        //    .AddClasses(c => c.AssignableTo(typeof(IAggregateRepository<>)))
        //    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
        //    .AsImplementedInterfaces()
        //    .WithScopedLifetime());

        services.Scan(scan => scan
        .FromApplicationDependencies()
        .AddClasses(c => c.AssignableTo(typeof(IAggregateRepository<>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime());

        return services;
    }

    public static IServiceCollection AddScrutorDI(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.Scan(scan => scan
            .FromAssemblies(assemblies)

            .AddClasses(c => c.AssignableTo<IScoped>()).UsingRegistrationStrategy(RegistrationStrategy.Skip).AsImplementedInterfaces().WithScopedLifetime()
            .AddClasses(c => c.AssignableTo<ITransient>()).UsingRegistrationStrategy(RegistrationStrategy.Skip).AsImplementedInterfaces().WithTransientLifetime()
            .AddClasses(c => c.AssignableTo<ISingleton>()).UsingRegistrationStrategy(RegistrationStrategy.Skip).AsImplementedInterfaces().WithSingletonLifetime());

        return services;
    }
    public static IServiceCollection AddSelfScrutorSelfDI(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.Scan(scan => scan
        .FromAssemblies(assemblies)
        .AddClasses(c => c.AssignableTo<IScoped>()).UsingRegistrationStrategy(RegistrationStrategy.Skip).AsSelfWithInterfaces().WithScopedLifetime()
        .AddClasses(c => c.AssignableTo<ITransient>()).UsingRegistrationStrategy(RegistrationStrategy.Skip).AsSelfWithInterfaces().WithTransientLifetime()
        .AddClasses(c => c.AssignableTo<ISingleton>()).UsingRegistrationStrategy(RegistrationStrategy.Skip).AsSelfWithInterfaces().WithSingletonLifetime());

        return services;
    }

    public static IServiceCollection AddSingleton<TInterface, TImplementation>(this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        services.AddSingleton<TInterface, TImplementation>();

        return services;
    }

    public static IServiceCollection AddScoped<TInterface, TImplementation>(this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        services.AddScoped<TInterface, TImplementation>();

        return services;
    }

    public static IServiceCollection AddTransient<TInterface, TImplementation>(this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        services.AddTransient<TInterface, TImplementation>();

        return services;
    }
}
