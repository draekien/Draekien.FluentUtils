using System;

namespace FluentUtils.MediatR.Pagination
{
    /// <summary>
    /// Navigation links in the paginated API response.
    /// </summary>
    public class Links
    {
        /// <summary>
        /// Gets or sets the URI link to the current page.
        /// </summary>
        public Uri Self { get; set; } = null!;
        /// <summary>
        /// Gets or sets the URI link to the next page.
        /// </summary>
        public Uri? Next { get; set; }
        /// <summary>
        /// Gets or sets the URI link to the previous page.
        /// </summary>
        public Uri? Previous { get; set; }
    }
}
