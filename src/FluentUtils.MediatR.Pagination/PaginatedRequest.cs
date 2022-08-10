using System.ComponentModel;
using MediatR;

namespace FluentUtils.MediatR.Pagination
{
    /// <summary>
    /// The base request object for a paginated API request.
    /// </summary>
    /// <typeparam name="TResult">The return data type for the results IEnumerable.</typeparam>
    public abstract class PaginatedRequest<TResult> : IRequest<PaginatedResponse<TResult>>
    {
        private int? _limit;

        /// <summary>
        /// Gets or initializes the maximum number of records to return.
        /// </summary>
        [DefaultValue(10)]
        public int Limit
        {
            get => _limit ?? 10;
            set
            {
                _limit = value switch
                {
                    < 1 => 10,
                    > 100 => 100,
                    var _ => value,
                };
            }
        }

        /// <summary>
        /// Gets or initializes the offset from 0.
        /// </summary>
        public int Offset { get; set; }
    }
}
