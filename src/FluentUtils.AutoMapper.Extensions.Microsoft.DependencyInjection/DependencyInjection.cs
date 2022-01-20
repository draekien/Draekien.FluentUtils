using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentUtils.AutoMapper;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     Extensions for registering AutoMapper profiles created with IMapFrom.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        ///     Registers AutoMapper using the assemblies containing implementations of IMapFrom.
        /// </summary>
        /// <param name="services">The current service collection.</param>
        /// <param name="assemblies">The assemblies containing implementations of IMapFrom.</param>
        /// <returns>The current service collection.</returns>
        /// <exception cref="ArgumentNullException">No assemblies were provided.</exception>
        public static IServiceCollection AddAutoMapperProfiles(
            this IServiceCollection services,
            params Assembly[]? assemblies)
        {
            if (assemblies is null) throw new ArgumentNullException(nameof(assemblies));

            List<Assembly> assembliesList = assemblies.ToList();
            assembliesList.Add(typeof(DependencyInjection).Assembly);
            assembliesList.Add(typeof(IMapFrom<>).Assembly);

            MappingProfile.AssembliesUsingIMapFrom = assembliesList;
            services.AddAutoMapper(assembliesList);

            return services;
        }
    }
}
