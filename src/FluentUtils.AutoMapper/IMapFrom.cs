using AutoMapper;

namespace FluentUtils.AutoMapper;

/// <summary>
///     Indicates a mapping relationship between the source type T and the current type
///     (class implementing this interface).
/// </summary>
/// <typeparam name="TSource">The source type.</typeparam>
public interface IMapFrom<TSource>
{
    /// <summary>
    ///     Creates a default mapping profile from the source type T to the destination type
    ///     (class that implements this interface).
    /// </summary>
    /// <param name="profile">The AutoMapper <see cref="Profile"/></param>
    void Mapping(Profile profile) => profile.CreateMap(typeof(TSource), GetType());
}
