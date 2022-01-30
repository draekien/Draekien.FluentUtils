using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FluentUtils.MinimalApis.EndpointDefinitions;

/// <summary>
///     Dependency Injection for EndpointDefinitions.
/// </summary>
public static class EndpointDefinitionsInjection
{
    /// <summary>
    ///     Scans the provided assemblies for classes implementing the <see cref="IEndpointDefinition"/> interface
    ///     and registers the services defined in the `DefineServices` method.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assembliesContainingDefinitions">The array of assemblies containing implementations of <see cref="IEndpointDefinition"/>.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddEndpointDefinitions(
        this IServiceCollection services,
        params Assembly[] assembliesContainingDefinitions)
    {
        List<IEndpointDefinition> endpointDefinitions = new();

        foreach (Assembly assembly in assembliesContainingDefinitions)
        {
            IEnumerable<Type> assignableTypes =
                assembly.ExportedTypes.Where(type => typeof(IEndpointDefinition).IsAssignableFrom(type) && !type.IsInterface);

            IEnumerable<IEndpointDefinition> activatedDefinitions = assignableTypes.Select(Activator.CreateInstance).Cast<IEndpointDefinition>();

            endpointDefinitions.AddRange(activatedDefinitions);
        }

        foreach (IEndpointDefinition endpointDefinition in endpointDefinitions)
        {
            endpointDefinition.DefineServices(services);
        }

        services.AddSingleton(endpointDefinitions as IReadOnlyCollection<IEndpointDefinition>);

        return services;
    }

    /// <summary>
    ///     Uses the endpoint definitions registered in the service collection to define the endpoints of the api.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The web application.</returns>
    public static WebApplication UseEndpointDefinitions(this WebApplication app)
    {
        var definitions = app.Services.GetRequiredService<IReadOnlyCollection<IEndpointDefinition>>();

        foreach (IEndpointDefinition endpointDefinition in definitions)
        {
            endpointDefinition.DefineEndpoints(app);
        }

        return app;
    }
}
