using System.Collections.Generic;

namespace FluentUtils.MediatR.Pagination
{
    /// <summary>
    /// The response object for paginated API requests.
    /// </summary>
    /// <typeparam name="TResult">The return data type for the results IEnumerable.</typeparam>
    public class PaginatedResponse<TResult>
    {
        /// <summary>
        /// Gets or sets the navigation links for pagination.
        /// </summary>
        public Links Links { get; set; } = null!;
        /// <summary>
        /// Gets or sets the limit used in the original request.
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// Gets or sets the offset used in the original request.
        /// </summary>
        public int Offset { get; set; }
        /// <summary>
        /// Gets or sets the total number of records in the results set.
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// Gets or sets the results set.
        /// </summary>
        public IEnumerable<TResult> Results { get; set; } = new List<TResult>();
    }
}
