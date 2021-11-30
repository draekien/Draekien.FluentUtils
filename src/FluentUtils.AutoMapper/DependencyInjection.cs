using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace FluentUtils.AutoMapper;

/// <summary>
///
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddAutoMapperProfiles(
        this IServiceCollection services,
        params Assembly[]? assemblies)
    {
        if (assemblies is null) throw new ArgumentNullException(nameof(assemblies));

        List<Assembly> assembliesList = assemblies.ToList();
        assembliesList.Add(Assembly.GetExecutingAssembly());

        MappingProfile.AssembliesUsingIMapFrom = assembliesList;
        services.AddAutoMapper(assembliesList);

        return services;
    }
}
