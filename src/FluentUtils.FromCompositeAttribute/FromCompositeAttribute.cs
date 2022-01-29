using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FluentUtils.FromCompositeAttribute
{
    /// <summary>
    /// Allows binding of properties from both the Path (FromRoute) and the Querystring (FromQuery).
    /// </summary>
    /// <remarks>
    /// You must configure the request object's properties that you want to bind to path / query.
    /// </remarks>
    public sealed class FromCompositeAttribute : Attribute, IBindingSourceMetadata
    {
        /// <inheritdoc />
        public BindingSource BindingSource { get; } = CompositeBindingSource.Create(
            new[] { BindingSource.Path, BindingSource.Query },
            nameof(FromCompositeAttribute)
        );
    }
}
