using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FluentUtils.MinimalApis.EndpointDefinitions;

/// <summary>
///     An endpoint definition for minimal apis.
/// </summary>
public interface IEndpointDefinition
{
    /// <summary>
    ///     Defines the endpoints to map in the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    void DefineEndpoints(WebApplication app);
    /// <summary>
    ///     Defines the services to register in the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    void DefineServices(IServiceCollection services);
}
