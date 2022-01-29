using System;
using System.Collections.Generic;
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
        private static readonly List<BindingSource> BindingSources = new()
        {
            BindingSource.Path,
            BindingSource.Query
        };

        /// <inheritdoc />
        public BindingSource BindingSource { get; } = CompositeBindingSource.Create(
            BindingSources,
            nameof(FromCompositeAttribute)
        );
    }
}
