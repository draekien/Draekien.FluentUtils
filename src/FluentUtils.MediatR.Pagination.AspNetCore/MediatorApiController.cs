namespace FluentUtils.MediatR.Pagination
{
    using System;
    using System.Net.Mime;
    using global::MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    ///     An API controller containing Mediator and Pagination.
    /// </summary>
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]")]
    public abstract class MediatorApiController : ControllerBase
    {
        private IMediator? _mediator;

        /// <summary>
        ///     Gets the mediator service.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     The mediator services has not been
        ///     registered.
        /// </exception>
        protected IMediator Mediator
        {
            get
            {
                if (_mediator is not null)
                {
                    return _mediator;
                }

                var mediator =
                    HttpContext.RequestServices.GetService<IMediator>();

                _mediator = mediator
                            ?? throw new InvalidOperationException(
                                "MediatR has not been registered in the current services collection."
                            );

                return _mediator;
            }
        }

        /// <summary>
        ///     Creates the navigation links for a paginated API response.
        /// </summary>
        /// <param name="actionName">The name of the current action.</param>
        /// <param name="request">The paginated API request.</param>
        /// <param name="response">The paginated API response.</param>
        /// <typeparam name="TResult">
        ///     The type of the results IEnumerable in the paginated
        ///     API response.
        /// </typeparam>
        /// <returns>The pagination links.</returns>
        protected Links CreatePaginationLinks<TResult>(
            string actionName,
            PaginatedRequest<TResult> request,
            PaginatedResponse<TResult> response
        ) => new()
        {
            Self = CreateLinkRelativeUri(actionName, request),
            Next = CreateNextLink(actionName, request, response),
            Previous = CreatePreviousLink(actionName, request, response),
        };

        /// <summary>
        ///     Creates the navigation link for the next page of results.
        /// </summary>
        /// <param name="actionName">The name of the current action.</param>
        /// <param name="request">The paginated API request.</param>
        /// <param name="response">The paginated API response.</param>
        /// <typeparam name="TResult">
        ///     The type of the results IEnumerable in the paginated
        ///     API response.
        /// </typeparam>
        /// <returns>The link to the next page if the next page exists.</returns>
        private Uri? CreateNextLink<TResult>(
            string actionName,
            PaginatedRequest<TResult> request,
            PaginatedResponse<TResult> response
        )
        {
            if (request.Offset + request.Limit >= response.Total) return null;

            request.Offset += request.Limit;

            Uri next = CreateLinkRelativeUri(actionName, request);

            request.Offset -= request.Limit;

            return next;
        }

        /// <summary>
        ///     Creates the navigation link for the previous page of results.
        /// </summary>
        /// <param name="actionName">The name of the current action.</param>
        /// <param name="request">The paginated API request.</param>
        /// <param name="response">The paginated API response.</param>
        /// <typeparam name="TResult">
        ///     The type of the results IEnumerable in the paginated
        ///     API response.
        /// </typeparam>
        /// <returns>The link to the previous page if the previous page exists.</returns>
        private Uri? CreatePreviousLink<TResult>(
            string actionName,
            PaginatedRequest<TResult> request,
            PaginatedResponse<TResult> response
        )
        {
            if (request.Offset <= 0) return null;

            if (request.Offset <= request.Limit)
            {
                request.Offset = 0;
                return CreateLinkRelativeUri(actionName, request);
            }

            if (request.Offset > response.Total)
            {
                request.Offset = response.Total - request.Limit;
                return CreateLinkRelativeUri(actionName, request);
            }

            request.Offset -= request.Limit;
            return CreateLinkRelativeUri(actionName, request);
        }

        /// <summary>
        ///     Creates the relative URI for the paginated request.
        /// </summary>
        /// <param name="actionName">The name of the current action.</param>
        /// <param name="request">The paginated API request.</param>
        /// <typeparam name="TResult">
        ///     The type of the results IEnumerable in the paginated
        ///     API response.
        /// </typeparam>
        /// <returns>The relative URI.</returns>
        private Uri CreateLinkRelativeUri<TResult>(
            string actionName,
            PaginatedRequest<TResult> request
        )
        {
            string? action = Url.Action(actionName, request);

            if (string.IsNullOrWhiteSpace(action))
            {
                throw new InvalidOperationException(
                    $"Cannot create link from action {actionName}"
                );
            }

            return new Uri(action, UriKind.Relative);
        }
    }
}
