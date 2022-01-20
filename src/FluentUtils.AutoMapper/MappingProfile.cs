using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace FluentUtils.AutoMapper
{

/// <summary>
///     A helper class for registering all types implementing IMapFrom
///     with AutoMapper.
/// </summary>
public class MappingProfile : Profile
{
    private const string MappingMethodName = "Mapping";
    private const string MappingInterface = "IMapFrom`1";

    /// <summary>
    ///     Creates a new mapping profile.
    /// </summary>
    public MappingProfile()
    {
        InvokeMappingsFromAssemblies(AssembliesUsingIMapFrom);
    }

    /// <summary>
    ///     An enumerable of assemblies that have classes which implement IMapFrom.
    ///     Used when registering the mapping profile.
    /// </summary>
    public static IEnumerable<Assembly> AssembliesUsingIMapFrom { get; set; } = Array.Empty<Assembly>();

    /// <summary>
    ///     Invokes the mapping methods found in the provided assemblies.
    /// </summary>
    /// <param name="assembliesUsingIMapFrom">The assemblies containing types that implement IMapFrom.</param>
    private void InvokeMappingsFromAssemblies(IEnumerable<Assembly> assembliesUsingIMapFrom)
    {
        foreach (Assembly assembly in assembliesUsingIMapFrom)
        {
            IEnumerable<Type> typesImplementingIMapFrom = GetTypesImplementingIMapFromInAssembly(assembly);
            InvokeMappingMethod(typesImplementingIMapFrom);
        }
    }

    /// <summary>
    ///     Gets the types which implement IMapFrom from the provided assembly.
    /// </summary>
    /// <param name="assembly">An assembly.</param>
    /// <returns>The list of types that implement IMapFrom.</returns>
    private static IEnumerable<Type> GetTypesImplementingIMapFromInAssembly(Assembly assembly)
    {
        Type[] exportedTypes = assembly.GetExportedTypes();
        return exportedTypes.Where(TypeImplementsIMapFrom());

        Func<Type, bool> TypeImplementsIMapFrom() => type => type.GetInterfaces().Any(InterfaceIsIMapFrom());
        Func<Type, bool> InterfaceIsIMapFrom() => @interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IMapFrom<>);
    }

    /// <summary>
    ///     Invokes the mapping method in the provided types.
    /// </summary>
    /// <param name="typesImplementingIMapFrom">The list of types that implement IMapFrom.</param>
    private void InvokeMappingMethod(IEnumerable<Type> typesImplementingIMapFrom)
    {
        foreach (Type type in typesImplementingIMapFrom)
        {
            object? instance = Activator.CreateInstance(type);
            MethodInfo? methodInfo = type.GetMethod(MappingMethodName)
                                  ?? type.GetInterface(MappingInterface)?.GetMethod(MappingMethodName);

            methodInfo?.Invoke(instance, new object?[] { this });
        }
    }
}

}
